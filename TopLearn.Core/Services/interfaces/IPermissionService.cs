using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.interfaces
{
    public interface IPermissionService
    {
        #region Role

        List<Role> GetRoles();
        Role GetRoleById(int id);
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditeRolesUser(List<int> roleIds, int userId);
        int AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);

        #endregion

        #region Permission

        List<Permission> GetAllPermission();
        void AddPermissionToRole(int roleId, List<int> permissions);
        List<int> GetPermissionsRole(int roleId);
        void UpdatePermissionRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string userName);

        #endregion
    }
}
