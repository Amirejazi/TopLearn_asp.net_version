using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;

        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationUserViewModel InformationUserViewModel { get; set; }

        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            InformationUserViewModel = _userService.GetUserInformation(id);
        }

        public IActionResult OnPost(int UserId)
        {
            _userService.DeleteUser(UserId);
            return Redirect("/Admin/Users");
        }
    }
}
