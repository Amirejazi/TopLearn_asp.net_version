using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Generator;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services
{
    public class CourseService : ICourseService
    {
        private TopLearnContext _context;

        public CourseService(TopLearnContext context)
        {
            _context = context;
        }

        public List<CourseGroup> getAllGroups()
        {
            return _context.CourseGroups.ToList();
        }

        public List<SelectListItem> GetGroupsForManage()
        {
            return _context.CourseGroups.Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupsForManage(int groupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserToRoles.Where(r => r.RoleId == 2).Include(r => r.User)
                .Select(u => new SelectListItem()
                {
                    Value = u.UserId.ToString(),
                    Text = u.User.UserName

                }).ToList();
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels.Select(l => new SelectListItem()
            {
                Value = l.LevelId.ToString(),
                Text = l.LevelTitle
            }).ToList();
        }

        public List<SelectListItem> GetStatus()
        {
            return _context.CourseStatus.Select(s => new SelectListItem()
            {
                Value = s.StatusId.ToString(),
                Text = s.StatusTitle
            }).ToList();
        }

        public int AddCourse(Course course, IFormFile imgCourse, IFormFile demoCourse)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "No_image.png";
            if (imgCourse != null)
            {
                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images/course/image",
                    course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }
            }

            if (demoCourse != null)
            {
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(demoCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images/course/demoes",
                    course.DemoFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    demoCourse.CopyTo(stream);
                }
            }

            _context.Add(course);
            _context.SaveChanges();
            return course.CourseId;
        }

        public List<ShowCourseForAdminViewModel> GetCoursesForAdmin()
        {
            return _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                CourseId = c.CourseId,
                CourseTitle = c.CourseTitle,
                ImageName = c.CourseImageName,
                EpisodeCount = c.CourseEpisodes.Count,
            }).ToList();
        }
    }
}
