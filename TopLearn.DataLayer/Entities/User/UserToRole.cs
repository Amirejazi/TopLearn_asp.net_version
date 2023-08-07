using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.User
{
    public class UserToRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }


        #region Relations

        public User User { get; set; }
        public Role Role { get; set; }

        #endregion
    }
}
