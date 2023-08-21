using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services.interfaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> getAllGroups();

        #endregion
    }
}
