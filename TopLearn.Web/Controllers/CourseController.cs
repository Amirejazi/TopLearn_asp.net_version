using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Archives;
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
        public IActionResult ShowCourse(int id, int episode=0)
        {
            Course course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }
            if( episode != 0)
                ViewBag.NotDemo = true;
            if (episode != 0 && User.Identity.IsAuthenticated)
            {
                if (!course.CourseEpisodes.Any(e => e.EpisodeId == episode))
                {
                    return NotFound();
                }

                if (!course.CourseEpisodes.First(e => e.EpisodeId ==episode).IsFree)
                {
                    if (!_orderService.IsUserInCourse(User.Identity.Name, id))
                    {
                        return NotFound();
                    }
                }

                var ep = course.CourseEpisodes.First(e => e.EpisodeId == episode);
                ViewBag.episode = ep;
                string filePath = "";
                string checkFilePath = Directory.GetCurrentDirectory();
                if (ep.IsFree)
                {
                    filePath = "/CourseOnline/" + ep.EpisodeFileName.Replace(".rar", ".mp4");
                    checkFilePath = Path.Combine(checkFilePath, "wwwroot/CourseOnline",
                        ep.EpisodeFileName.Replace(".rar", ".mp4"));
                }
                else
                {
                    filePath = "/CourseFilesOnline/" + ep.EpisodeFileName.Replace(".rar", ".mp4");
                    checkFilePath = Path.Combine(checkFilePath, "wwwroot/CourseFilesOnline",
                        ep.EpisodeFileName.Replace(".rar", ".mp4"));
                } 
                if (!System.IO.File.Exists(checkFilePath))
                {
                    string targetPath = Directory.GetCurrentDirectory();
                    if (ep.IsFree)
                    {
                        targetPath = Path.Combine(targetPath, "wwwroot/CourseOnline");
                    }
                    else
                    {
                        targetPath = Path.Combine(targetPath, "wwwroot/CourseFilesOnline");
                    }
                    string rarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/coursefiles", ep.EpisodeFileName);
                    var archive = ArchiveFactory.Open(rarPath);
                    var entries = archive.Entries.OrderBy(x => x.Key.Length);
                    foreach (var en in entries)
                    {
                        if (Path.GetExtension(en.Key) == ".mp4")
                        {
                            en.WriteTo(System.IO.File.Create(Path.Combine(targetPath, ep.EpisodeFileName.Replace(".rar", ".mp4"))));
                        }
                    }
                }
                ViewBag.filePath = filePath;
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

        public IActionResult CourseVote(int id)
        {
            if (!_courseService.IsFree(id) && User.Identity.IsAuthenticated)
            {
                if (!_orderService.IsUserInCourse(User.Identity.Name, id))
                {
                    ViewBag.NotAccess = true;
                }
            }
            return PartialView(_courseService.GetCourseVote(id));
        }

        [Authorize]
        public IActionResult AddVote(int id, bool vote)
        {
            _courseService.AddVote(_userService.GetUserIdByUserName(User.Identity.Name), id, vote);
            return PartialView("CourseVote", _courseService.GetCourseVote(id));
        }
    }
}
