using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseEpisode
    {
        [Key]
        public int EpisodeId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Display(Name = "عنوان قسمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string EpisodeTitle { get; set; }

        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EpisodeTime { get; set; }

        [Display(Name = "فایل")]
        public string EpisodeFileName { get; set; }

        [Display(Name = "رایگان بودن")]
        public bool IsFree { get; set; }

        #region Relation

        public Course Course { get; set; }

        #endregion
    }
}
