using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.User
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string RoleTitle { get; set; }


        #region Relations

        public List<UserToRole> UserToRoles { get; set; }

        #endregion
    }
}
