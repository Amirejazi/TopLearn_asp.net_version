using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Senders;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }

        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "این نام کاربری قبلا استفاده شده است!");
                return View(register);
            }
            if (_userService.IsExistEmail(FixedText.FixEmail(register.Email)))
            {
                ModelState.AddModelError("UserName", "این ایمیل قبلا استفاده شده است!");
                return View(register);
            }
            User user = new User()
            {
                UserName = register.UserName,
                Email = FixedText.FixEmail(register.Email),
                ActiveCode = NameGenerator.GenerateUniqCode(),
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                UserAvatar = "Default.jpg",
                RegisteredDate = DateTime.Now
            };
            _userService.AddUser(user);
            #region Send Validation Email

            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            EmailSender.SendEmailAsync(user.Email, "فعالسازی", body);

            #endregion
            return View("SuccessRegister", user);
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = _userService.LoginUser(login);
            if (user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات وارد شده نادرست میباشد!");
                return View(login);
            }
            if (!user.IsActive)
            {
                ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد!");
                return View(login);
            }
            var claims = new List<Claim>(){
                new Claim (ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim (ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.ReMemberMe
            };
            HttpContext.SignInAsync(principal, properties);

            ViewData["IsSuccess"] = true;
            return View(login);
        }

        #endregion

        #region ActiveAcount

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }
        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

        #region Forgot Password

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            string fixedEmail = FixedText.FixEmail(forgot.Email);
            var user = _userService.GetUserByEmail(fixedEmail);
            if(user == null)
            {
                ModelState.AddModelError("Email", "کاربری با این ایمیل یافت نشد!");
                return View(forgot);
            }
            string body = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            EmailSender.SendEmailAsync(user.Email, "بازیابی رمز عبور", body);
            ViewData["IsSuccess"] = true;
            return View(forgot);
        }

        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPassworViewModel()
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassworViewModel reset)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var user = _userService.GetUserByActiveCode(reset.ActiveCode);
            if (user == null)
            {
                return NotFound();
            }
            string hashpassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hashpassword;
            _userService.UpdateUser(user);
            ViewData["IsSuccess"] = true;
            return View(reset);
        }

        #endregion

    }
}
