using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    //[PermissionChecker(12)]
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;

        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet(int id)
        {
            Course = _courseService.GetCourseById(id);

            var groups = _courseService.GetGroupsForManage();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

            List<SelectListItem> subGroups = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید", Value = ""},
            };
            subGroups.AddRange(_courseService.GetSubGroupsForManage(Course.GroupId));
            string selectedSubGroup = null;
            if (Course.SubGroup != null)
                selectedSubGroup = Course.SubGroup.ToString();
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text", selectedSubGroup);

            var teachers = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text", Course.TeacherId);

            var levels = _courseService.GetLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text", Course.LevelId);

            var status = _courseService.GetStatus();
            ViewData["Status"] = new SelectList(status, "Value", "Text", Course.StatusId);
        }

        public IActionResult OnPost(IFormFile imgCourseUp, IFormFile demoUp)
        {

            _courseService.UpdateCourse(Course, imgCourseUp, demoUp);

            return RedirectToPage("Index");
        }
    }
}
