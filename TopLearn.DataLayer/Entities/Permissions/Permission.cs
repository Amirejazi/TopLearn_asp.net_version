using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Permissions
{
    public class Permission
    {
        [Key]
        public int PermiossionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} باشد .")]
        public string PermissionTitle { get; set; }

        public int? ParentID { get; set; }

        #region Relation

        [ForeignKey("ParentID")]
        public List<Permission> Type { get; set; }
        public List<RoleToPermission> RoleToPermissions { get; set; }

        #endregion

    }
}
