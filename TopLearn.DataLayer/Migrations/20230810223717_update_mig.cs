using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TopLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class update_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Users_UserId",
                table: "Wallet");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_WalletType_WalletTypeTypeId",
                table: "Wallet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletType",
                table: "WalletType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallet",
                table: "Wallet");

            migrationBuilder.RenameTable(
                name: "WalletType",
                newName: "WalletTypes");

            migrationBuilder.RenameTable(
                name: "Wallet",
                newName: "Wallets");

            migrationBuilder.RenameIndex(
                name: "IX_Wallet_WalletTypeTypeId",
                table: "Wallets",
                newName: "IX_Wallets_WalletTypeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Wallet_UserId",
                table: "Wallets",
                newName: "IX_Wallets_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletTypes",
                table: "WalletTypes",
                column: "TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "WalletId");

            migrationBuilder.InsertData(
                table: "WalletTypes",
                columns: new[] { "TypeId", "TypeTitle" },
                values: new object[,]
                {
                    { 1, "واریز" },
                    { 2, "برداشت" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypeTypeId",
                table: "Wallets",
                column: "WalletTypeTypeId",
                principalTable: "WalletTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UserId",
                table: "Wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypeTypeId",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletTypes",
                table: "WalletTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.DeleteData(
                table: "WalletTypes",
                keyColumn: "TypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WalletTypes",
                keyColumn: "TypeId",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "WalletTypes",
                newName: "WalletType");

            migrationBuilder.RenameTable(
                name: "Wallets",
                newName: "Wallet");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_WalletTypeTypeId",
                table: "Wallet",
                newName: "IX_Wallet_WalletTypeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_UserId",
                table: "Wallet",
                newName: "IX_Wallet_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletType",
                table: "WalletType",
                column: "TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallet",
                table: "Wallet",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Users_UserId",
                table: "Wallet",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_WalletType_WalletTypeTypeId",
                table: "Wallet",
                column: "WalletTypeTypeId",
                principalTable: "WalletType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
