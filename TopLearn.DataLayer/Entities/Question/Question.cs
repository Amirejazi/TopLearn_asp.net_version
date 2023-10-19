using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Question
{
    public class Question
    {
        [Key]
        public int  QuestionId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Display(Name = "عنوان سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string Title { get; set; }
        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Body { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime Modified { get; set; }

        #region Relations

        public Course.Course Course { get; set; }
        public User.User User { get; set; }
        public List<Answer> Answers { get; set; }

        #endregion
    }
}
