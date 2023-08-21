using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private TopLearnContext _context;

        public PermissionService(TopLearnContext context)
        {
            _context = context;
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.Find(id);
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var roleId in roleIds)
            {
                _context.UserToRoles.Add(new UserToRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public void EditeRolesUser(List<int> roleIds, int userId)
        {
            // Delete All Roles User
            _context.UserToRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserToRoles.Remove(r));

            // Add new Roles
            AddRolesToUser(roleIds, userId);
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            _context.SaveChanges();
        }

        public List<Permission> GetAllPermission()
        {
            return _context.Permissions.ToList();
        }

        public void AddPermissionToRole(int roleId, List<int> permissions)
        {
            foreach (var p in permissions)
            {
                _context.RoleToPermissions.Add(new RoleToPermission()
                {
                    RoleId = roleId,
                    PermissionId = p
                });
            }

            _context.SaveChanges();
        }

        public List<int> GetPermissionsRole(int roleId)
        {
            return _context.RoleToPermissions.Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId).ToList();
        }

        public void UpdatePermissionRole(int roleId, List<int> permissions)
        {
            _context.RoleToPermissions.Where(r => r.RoleId ==roleId).ToList().ForEach(r => _context.Remove(r));

            AddPermissionToRole(roleId, permissions);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId = _context.Users.Single(u => u.UserName == userName).UserId;
            List<int> UserRoles = _context.UserToRoles.Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();

            if(!UserRoles.Any())
                return false;

            List<int> RolesPermission = _context.RoleToPermissions.Where(rp => rp.PermissionId == permissionId)  // جمع آوری نقش هایی که دارای این پرمیژن هستن
                .Select(rp => rp.RoleId).ToList();

            return RolesPermission.Any(r => UserRoles.Contains(r));




        }
    }
}
