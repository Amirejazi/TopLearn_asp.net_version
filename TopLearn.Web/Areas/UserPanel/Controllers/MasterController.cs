using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MasterController : Controller
    {
        #region Ctor

        private ICourseService _courseService;
        private IUserService _userService;

        public MasterController(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        #endregion

        [HttpGet("master-courses")]
        public IActionResult MasterCoursesList()
        {
            var courses = _courseService.GetAllMasterCourses(User.Identity.Name);
            return View(courses);
        }

        [HttpGet("master-episodes/{course_id}")]
        public IActionResult EpisodeList(int course_id)
        {
            var course = _courseService.GetCourseById(course_id);
            if (course == null)
            {
                return NotFound();
            }

            if (course.TeacherId != _userService.GetUserIdByUserName(User.Identity.Name))
            {
                return RedirectToAction("MasterCoursesList", "Master");
            }

            var episodes = _courseService.GetEpisodesByCourseId(course_id);
            ViewBag.CourseId = course_id;
            return View(episodes);
        }

        [HttpGet("add-episode/{course_id}")]
        public IActionResult AddEpisode(int course_id)
        {
            var course = _courseService.GetCourseById(course_id);
            if (course == null)
            {
                return NotFound();
            }

            if (course.TeacherId != _userService.GetUserIdByUserName(User.Identity.Name))
            {
                return RedirectToAction("MasterCoursesList", "Master");
            }
            var addEpisode = new AddEpisodeViewModel()
            {
                CourseId = course.CourseId
            };
            return View(addEpisode);
        }

        [HttpPost("add-episode/{course_id}")]
        public IActionResult AddEpisode(AddEpisodeViewModel addEpisode)
        {
            if (!ModelState.IsValid)
            {
                return View(addEpisode);
            }
            if (string.IsNullOrEmpty(addEpisode.EpisodeFileName))
            {
                return View(addEpisode);
            }

            var result = _courseService.AddEpisode(addEpisode, User.Identity.Name);
            if (result)
            {
                return RedirectToAction("EpisodeList", "Master", new { course_id = addEpisode.CourseId });
            }
            return View(addEpisode);
        }

        [HttpPost("dropzone-target/{courseId}")]
        public IActionResult DropzoneTarget(List<IFormFile> files, int courseId)
        {
            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    string fileName = $"{courseId}-{Guid.NewGuid()}" + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/coursefiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string finalPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(finalPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return new JsonResult(new { data = fileName, status = "Success" });
                }
            }
            return new JsonResult(new { status = "Error" });
        }
    }
}
