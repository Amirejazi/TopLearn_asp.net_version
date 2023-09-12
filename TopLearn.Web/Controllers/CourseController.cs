using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;

        public CourseController(ICourseService courseService, IOrderService orderService)
        {
            _courseService = courseService;
            _orderService = orderService;
        }

        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroup = null)
        {
            ViewBag.SelectedGroups = selectedGroup;
            ViewBag.Groups = _courseService.getAllGroups();
            ViewBag.pageId = pageId;
            return View(_courseService.GetCourses(pageId, filter, getType, orderByType, startPrice, endPrice, selectedGroup, 9));
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id)
        {
            Course course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            int orderId = _orderService.AddOrder(User.Identity.Name, id);
            return Redirect($"/UserPanel/MyOrder/ShowOrder/{orderId}");
        }
    }
}
