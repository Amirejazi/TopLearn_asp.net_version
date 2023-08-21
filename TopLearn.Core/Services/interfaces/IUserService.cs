using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        bool ActiveAccount(string activeCode);
        User GetUserByEmail(string email);
        User GetUserById(int id);
        User GetUserByUserName(string userName);
        User GetUserByActiveCode(string activeCode);
        void UpdateUser(User user);
        int GetUserIdByUserName(string userName);
        void DeleteUser(int id);

        #region UserPanel

        InformationUserViewModel GetUserInformation(string userName); 
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSideBarUserPanel(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string userName);
        void EditProfile(string userName, EditProfileViewModel editProfile);
        bool CompareOldPassword(string userName, string oldPassword);
        void ChangeUserPassword(string userName, string newPassword);

        #endregion

        #region Wallet

        int BalanceWallet(string userName);
        List<WalletViewModel> GetWalletsUser(string userName);
        int ChargeWallet(string userName, string description, int amount, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);

        #endregion

        #region Admin Panel

        UsersForAdminViewModel GetUsers(int pageId=1, string filterEmail="", string filterUserName="");
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);
        UsersForAdminViewModel GetDeletedUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        #endregion
    }
}
