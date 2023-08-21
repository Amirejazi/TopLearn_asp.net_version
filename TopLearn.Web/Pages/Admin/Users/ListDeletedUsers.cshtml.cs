using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class ListDeletedUsersModel : PageModel
    {
        private IUserService _userService;

        public ListDeletedUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public UsersForAdminViewModel UsersForAdminViewModel { get; set; }

        public void OnGet(int pageId = 1, string emailfilter = "", string userNamefliter = "")
        {
            UsersForAdminViewModel = _userService.GetDeletedUsers(pageId, emailfilter, userNamefliter);
        }
    }
}
