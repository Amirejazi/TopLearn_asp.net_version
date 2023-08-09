using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
