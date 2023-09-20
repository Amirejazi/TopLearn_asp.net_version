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
        private IUserService _userService;

        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
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

        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFiles(int episodeId)
        {
            var episode = _courseService.GetEpisodeById(episodeId);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coursefiles", episode.EpisodeFileName);
            string fileName = episode.EpisodeFileName;

            if (episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                return File(file, "application/force-download", fileName);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_orderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filePath);
                    return File(file, "application/force-download", fileName);
                }
            }

            return Forbid();
        }

        [HttpPost]
        public IActionResult CreateComment(CourseComment comment)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.User = _userService.GetUserByUserName(User.Identity.Name);
            _courseService.AddComment(comment);

            return View("ShowComments", _courseService.getCourseComments(comment.CourseId));
        }

        public IActionResult ShowComments(int id, int pageId = 1)
        {
            return View(_courseService.getCourseComments(id, pageId));
        }
    }
}
