using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Course
{
    public class UserCourse
    {
        [Key]
        public int UC_Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }

        #region Relations

        public Course Course { get; set; }
        public User.User User { get; set; }

        #endregion
    }
}
