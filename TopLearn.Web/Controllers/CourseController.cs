using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroup = null)
        {
            ViewBag.SelectedGroups = selectedGroup;
            ViewBag.Groups = _courseService.getAllGroups();
            ViewBag.pageId = pageId;
            return View(_courseService.GetCourses(pageId, filter, getType, orderByType, startPrice, endPrice, selectedGroup, 9));
        }
    }
}
