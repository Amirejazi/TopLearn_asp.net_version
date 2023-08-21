using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services
{
    public class UserService : IUserService
    {
        public TopLearnContext _context { get; set; }
        public UserService(TopLearnContext context)
        {
            _context = context;
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(login.Password);
            string email = FixedText.FixEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPassword);
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();
            return true;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);

        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public void DeleteUser(int id)
        {
            User user = GetUserById(id);
            user.IsDelete = true;
            UpdateUser(user);
        }

        public InformationUserViewModel GetUserInformation(string userName)
        {
            var user = GetUserByUserName(userName);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisteredDate = user.RegisteredDate;
            information.Wallet = BalanceWallet(userName);

            return information;
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisteredDate = user.RegisteredDate;
            information.Wallet = BalanceWallet(user.UserName);

            return information;
        }

        public SideBarUserPanelViewModel GetSideBarUserPanel(string userName)
        {
            var user = GetUserByUserName(userName);
            SideBarUserPanelViewModel sideBar = new SideBarUserPanelViewModel();
            sideBar.UserName = user.UserName;
            sideBar.RegisteredDate = user.RegisteredDate;
            sideBar.ImageName = user.UserAvatar;

            return sideBar;
        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).Select(u => new EditProfileViewModel()
            {
                UserName = u.UserName,
                Email = u.Email,
                ImageName = u.UserAvatar

            }).Single();
        }

        public void EditProfile(string userName, EditProfileViewModel editProfile)
        {
            if (editProfile.FormFile != null)
            {
                string imagePath = "";
                if (editProfile.ImageName != "Default.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                                            "wwwroot",
                                            "images/UserAvatar",
                                            editProfile.ImageName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
                editProfile.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editProfile.FormFile.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                                            "wwwroot",
                                            "images/UserAvatar",
                                            editProfile.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editProfile.FormFile.CopyTo(stream);
                }
            }

            var user = GetUserByUserName(userName);
            user.UserName = editProfile.UserName;
            user.Email = editProfile.Email;
            user.UserAvatar = editProfile.ImageName;

            UpdateUser(user);

        }

        public bool CompareOldPassword(string userName, string oldPassword)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserName == userName && u.Password == hashPassword);
        }

        public void ChangeUserPassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;
        }

        public int BalanceWallet(string userName)
        {
            int userId = GetUserIdByUserName(userName);

            var enter = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay == true)
                .Select(w => w.Amount).ToList();
            var exit = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 2)
                .Select(w => w.Amount).ToList();

            return (enter.Sum() - exit.Sum());

        }

        public List<WalletViewModel> GetWalletsUser(string userName)
        {
            int userId = GetUserIdByUserName(userName);

            return _context.Wallets
                .Where(w => w.UserId == userId && w.IsPay == true)
                .Select(w => new WalletViewModel()
                {
                    Amount = w.Amount,
                    Description = w.Description,
                    TypeId = w.TypeId,
                    DateTime = w.CreateDate
                })
                .ToList();
        }

        public int ChargeWallet(string userName, string description, int amount, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                IsPay = isPay,
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                TypeId = 1,
                UserId = GetUserIdByUserName(userName)
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        public UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users;  // این لیزی لوده و همه یوزر ها برگشت داده نمی شود

            if (!string.IsNullOrEmpty(filterEmail))
                result = result.Where(x => x.Email.Contains(filterEmail));

            if (!string.IsNullOrEmpty(filterUserName))
                result = result.Where(x => x.UserName.Contains(filterUserName));

            //show item in page

            int take = 10;
            int skip = (pageId - 1) * 10;

            UsersForAdminViewModel output = new UsersForAdminViewModel();
            output.Users = result.OrderBy(u => u.RegisteredDate).Skip(skip).Take(take).ToList();
            output.CurrentPage = pageId;
            output.PageCount = result.Count() / take;

            return output;

        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User adduser = new User();
            adduser.Email = user.Email;
            adduser.UserName = user.UserName;
            adduser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            adduser.IsActive = true;
            adduser.RegisteredDate = DateTime.Now;
            adduser.ActiveCode = NameGenerator.GenerateUniqCode();

            if (user.FormFile != null)
            {
                string imagePath = "";

                adduser.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.FormFile.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images/UserAvatar",
                    adduser.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.FormFile.CopyTo(stream);
                }
            }
            else
            {
                adduser.UserAvatar = "Default.jpg";
            }

            return AddUser(adduser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    AvatarName = u.UserAvatar,
                    Roles = u.UserToRoles.Select(r => r.RoleId).ToList()
                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            }

            if (editUser.FormFile != null)
            {
                if (editUser.AvatarName != "Default.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images/UserAvatar",
                        editUser.AvatarName);

                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }
                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.FormFile.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images/UserAvatar",
                    user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.FormFile.CopyTo(stream);
                }
            }

            UpdateUser(user);
        }

        public UsersForAdminViewModel GetDeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete);  // این لیزی لوده و همه یوزر ها برگشت داده نمی شود

            if (!string.IsNullOrEmpty(filterEmail))
                result = result.Where(x => x.Email.Contains(filterEmail));

            if (!string.IsNullOrEmpty(filterUserName))
                result = result.Where(x => x.UserName.Contains(filterUserName));

            //show item in page

            int take = 10;
            int skip = (pageId - 1) * 10;

            UsersForAdminViewModel output = new UsersForAdminViewModel();
            output.Users = result.OrderBy(u => u.RegisteredDate).Skip(skip).Take(take).ToList();
            output.CurrentPage = pageId;
            output.PageCount = result.Count() / take;

            return output;
        }
    }
}