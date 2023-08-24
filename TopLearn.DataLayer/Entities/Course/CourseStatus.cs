using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(150)]
        public string StatusTitle { get; set; }

        //Navigation Prperty
        public List<Course> Courses { get; set; }
    }
}
