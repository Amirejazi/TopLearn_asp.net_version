﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Generator;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;
using static TopLearn.Core.Security.ImageValidatore;

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
            if (imgCourse != null && imgCourse.IsImage())
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

                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/course/thumb", course.CourseImageName);
                ImageConvertor imgResizer = new ImageConvertor();
                imgResizer.Image_resize(imagePath, thumbPath, 150);
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

        public Course GetCourseById(int id) => _context.Courses.Find(id);

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demoCourse)
        {
            course.UpdateDate = DateTime.Now;

            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName != "No_image.png")
                {
                    string deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images/course/image",
                        course.CourseImageName);
                    if (File.Exists(deleteImagePath))
                    {
                        File.Delete(deleteImagePath);
                    }

                    string deleteThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images/course/thumb",
                        course.CourseImageName);
                    if (File.Exists(deleteThumbPath))
                    {
                        File.Delete(deleteThumbPath);
                    }
                }
                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images/course/image",
                    course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/course/thumb", course.CourseImageName);
                ImageConvertor imgResizer = new ImageConvertor();
                imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            if (demoCourse != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images/course/demoes",
                        course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(demoCourse.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images/course/demoes",
                    course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoCourse.CopyTo(stream);
                }
            }

            _context.Update(course);
            _context.SaveChanges();
        }

        public Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, string filter = "", string getType = "all", string orderByType = "date",
            int startPrice = 0, int endPrice = 0, List<int> selectedGroup = null, int take = 8)
        {
            IQueryable<Course> result = _context.Courses;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter)|| c.Tags.Contains(filter));
            }

            switch (getType)
            {
                case "all":
                    break;
                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "updatedate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > startPrice);
            }

            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < endPrice);
            }

            if (selectedGroup != null && selectedGroup.Any())
            {
                foreach (int groupId in selectedGroup)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroupId == groupId);
                }
            }
            int skip = (pageId - 1) * take;
            int pageCount = result.Count() / take;

            var output = result.Include(c => c.CourseEpisodes).Select(c => new ShowCourseListItemViewModel()
            {
                CourseId = c.CourseId,
                Title = c.CourseTitle,
                ImageName = c.CourseImageName,
                Price = c.CoursePrice,
                //TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Skip(skip).Take(take).ToList();

            return Tuple.Create(output, pageCount);
        }


        public List<CourseEpisode> GetListEpisodeCourse(int courseId)
        {
            return _context.CourseEpisodes.Where(e => e.CourseId == courseId).ToList();
        }

        public int AddEpisode(CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            courseEpisode.EpisodeFileName = episodeFile.FileName;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "coursefiles",
                courseEpisode.EpisodeFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                episodeFile.CopyTo(stream);
            }

            _context.CourseEpisodes.Add(courseEpisode);
            _context.SaveChanges();
            return courseEpisode.EpisodeId;
        }

        public bool CheckExistFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coursefiles", fileName);
            return File.Exists(filePath);
        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public void EditEpisode(CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coursefiles", courseEpisode.EpisodeFileName);
                File.Delete(deleteFilePath);

                courseEpisode.EpisodeFileName = episodeFile.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "coursefiles", courseEpisode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }

            _context.Update(courseEpisode);
            _context.SaveChanges();

        }

        public Course GetCourseForShow(int courseId )
        {
            return _context.Courses
                .Include(c => c.CourseEpisodes)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseStatus)
                .Include(c => c.User)
                .FirstOrDefault(c => c.CourseId == courseId);
        }
    }
}
