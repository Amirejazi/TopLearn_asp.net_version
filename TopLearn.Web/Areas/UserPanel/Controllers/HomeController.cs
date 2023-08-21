using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userservice;

        public HomeController(IUserService userService)
        {
            _userservice = userService;
        }

        public IActionResult Index()
        {
            return View(_userservice.GetUserInformation(User.Identity.Name));
        }

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userservice.GetDataForEditProfileUser(User.Identity.Name));
        }

        [HttpPost("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel editProfile)
        {
            if (!ModelState.IsValid)
            {
                return View(editProfile);
            }
            _userservice.EditProfile(User.Identity.Name, editProfile);
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string currentUserName = User.Identity.Name;
            if(!ModelState.IsValid)
                return View(changePassword);

            if (!_userservice.CompareOldPassword(currentUserName, changePassword.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی معتبر نمیباشد!");
                return View(changePassword);
            }

            _userservice.ChangeUserPassword(currentUserName, changePassword.Password);
            ViewBag.IsSuccess = true;
            return View();

        }

    }

}
