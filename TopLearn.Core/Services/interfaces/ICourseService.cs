using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.DTOs.Course;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services.interfaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> getAllGroups();
        List<SelectListItem> GetGroupsForManage();
        List<SelectListItem> GetSubGroupsForManage(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatus();

        #endregion

        #region Course

        int AddCourse(Course course, IFormFile imgCourse, IFormFile demoCourse);
        List<ShowCourseForAdminViewModel> GetCoursesForAdmin();
        Course GetCourseById(int id);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demoCourse);
        Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, string filter = "", string getType = "all", string orderByType="date", int startPrice = 0, int endPrice = 0, List<int> selectedGroup = null, int take=8);

        #endregion

        #region Episode

        List<CourseEpisode> GetListEpisodeCourse(int courseId);
        int AddEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        bool CheckExistFile(string fileName);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        Course GetCourseForShow(int courseId);

        #endregion
    }
}
