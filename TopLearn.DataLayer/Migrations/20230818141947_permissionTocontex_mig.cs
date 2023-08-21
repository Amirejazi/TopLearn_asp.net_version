using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class permissionTocontex_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Permission_ParentID",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleToPermission_Permission_PermissionId",
                table: "RoleToPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleToPermission_Roles_RoleId",
                table: "RoleToPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleToPermission",
                table: "RoleToPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "RoleToPermission",
                newName: "RoleToPermissions");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameIndex(
                name: "IX_RoleToPermission_PermissionId",
                table: "RoleToPermissions",
                newName: "IX_RoleToPermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_ParentID",
                table: "Permissions",
                newName: "IX_Permissions_ParentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleToPermissions",
                table: "RoleToPermissions",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "PermiossionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Permissions_ParentID",
                table: "Permissions",
                column: "ParentID",
                principalTable: "Permissions",
                principalColumn: "PermiossionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleToPermissions_Permissions_PermissionId",
                table: "RoleToPermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "PermiossionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleToPermissions_Roles_RoleId",
                table: "RoleToPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Permissions_ParentID",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleToPermissions_Permissions_PermissionId",
                table: "RoleToPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleToPermissions_Roles_RoleId",
                table: "RoleToPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleToPermissions",
                table: "RoleToPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "RoleToPermissions",
                newName: "RoleToPermission");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_RoleToPermissions_PermissionId",
                table: "RoleToPermission",
                newName: "IX_RoleToPermission_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_ParentID",
                table: "Permission",
                newName: "IX_Permission_ParentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleToPermission",
                table: "RoleToPermission",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "PermiossionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Permission_ParentID",
                table: "Permission",
                column: "ParentID",
                principalTable: "Permission",
                principalColumn: "PermiossionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleToPermission_Permission_PermissionId",
                table: "RoleToPermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "PermiossionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleToPermission_Roles_RoleId",
                table: "RoleToPermission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
