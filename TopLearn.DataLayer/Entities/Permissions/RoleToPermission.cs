using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Permissions
{
    public class RoleToPermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        #region Relation

        public Role Role { get; set; }
        public Permission Permission { get; set; }

        #endregion
    }
}
