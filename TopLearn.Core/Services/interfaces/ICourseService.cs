using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    }
}
