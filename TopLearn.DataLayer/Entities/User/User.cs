using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.User
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نمی باشد .")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string Password { get; set; }

        [Display(Name = "کد فعال سازی")]
        [MaxLength(50, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string ActiveCode { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string UserAvatar { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisteredDate { get; set; }

        public bool IsDelete { get; set; }

        #region Relations

        public List<UserToRole> UserToRoles { get; set; }
        public List<Wallet.Wallet> wallets { get; set; }
        public List<Course.Course> Courses { get; set; }

        #endregion

    }
}
