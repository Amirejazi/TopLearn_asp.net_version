using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permissions;

namespace TopLearn.DataLayer.Entities.User
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string RoleTitle { get; set; }

        public bool IsDelete { get; set; }

        #region Relations

        public List<UserToRole>? UserToRoles { get; set; }
        public List<RoleToPermission> RoleToPermissions { get; set; }

        #endregion
    }
}
