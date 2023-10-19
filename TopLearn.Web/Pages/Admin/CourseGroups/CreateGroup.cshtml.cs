using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    //[PermissionChecker(8)]
    public class CreateGroupModel : PageModel
    {
        private ICourseService _courseService;

        public CreateGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public CourseGroup CourseGroup { get; set; }

        public void OnGet(int? id)
        {
            CourseGroup = new CourseGroup()
            {
                ParentId = id
            };
        }

        public IActionResult OnPost()
        {
            _courseService.AddGroup(CourseGroup);
            return RedirectToPage("Index");
        }
    }
}
