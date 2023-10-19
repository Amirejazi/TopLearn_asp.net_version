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
        CourseGroup GetGroupById(int groupId);
        List<SelectListItem> GetGroupsForManage();
        List<SelectListItem> GetSubGroupsForManage(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatus();
        void AddGroup(CourseGroup group);
        void UpdateGroup(CourseGroup group);

        #endregion

        #region Course

        int AddCourse(Course course, IFormFile imgCourse, IFormFile demoCourse);
        List<ShowCourseForAdminViewModel> GetCoursesForAdmin();
        Course GetCourseById(int id);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demoCourse);
        Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, string filter = "", string getType = "all", string orderByType="date", int startPrice = 0, int endPrice = 0, List<int> selectedGroup = null, int take=8);
        List<ShowCourseListItemViewModel> GetPopularCourse();
        bool IsFree(int id);
        List<Course> GetAllMasterCourses(string userName);

        #endregion

        #region Episode

        List<CourseEpisode> GetListEpisodeCourse(int courseId);
        int AddEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        bool CheckExistFile(string fileName);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        Course GetCourseForShow(int courseId);
        List<CourseEpisode> GetEpisodesByCourseId(int courseId);
        bool AddEpisode(AddEpisodeViewModel addEpisode, string userName);
        #endregion

        #region Comments

        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>, int> getCourseComments(int courseId, int pageId=1);

        #endregion

        #region Course Vote

        void AddVote(int userId, int courseId, bool vote);
        Tuple<int, int> GetCourseVote(int courseId);

        #endregion
    }
}
