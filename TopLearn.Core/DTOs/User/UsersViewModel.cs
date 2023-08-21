using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.DTOs
{
    public class UsersForAdminViewModel
    {
        public List<User> Users { get; set;}
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class CreateUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد .")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string Password { get; set; }

        public IFormFile? FormFile { get; set; }
    }

    public class EditUserViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد .")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string Password { get; set; }

        public IFormFile? FormFile { get; set; }

        public List<int>? Roles { get; set; }

        public string AvatarName { get; set; }
    }
}
