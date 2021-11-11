using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class appusergrouppointsfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPoints_AppUserGroups_AppUserGroupId",
                table: "AppUserPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserPoints",
                table: "AppUserPoints");

            migrationBuilder.RenameTable(
                name: "AppUserPoints",
                newName: "AppUserGroupPoints");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserPoints_AppUserGroupId",
                table: "AppUserGroupPoints",
                newName: "IX_AppUserGroupPoints_AppUserGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserGroupPoints",
                table: "AppUserGroupPoints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroupPoints_AppUserGroups_AppUserGroupId",
                table: "AppUserGroupPoints",
                column: "AppUserGroupId",
                principalTable: "AppUserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroupPoints_AppUserGroups_AppUserGroupId",
                table: "AppUserGroupPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserGroupPoints",
                table: "AppUserGroupPoints");

            migrationBuilder.RenameTable(
                name: "AppUserGroupPoints",
                newName: "AppUserPoints");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserGroupPoints_AppUserGroupId",
                table: "AppUserPoints",
                newName: "IX_AppUserPoints_AppUserGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserPoints",
                table: "AppUserPoints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPoints_AppUserGroups_AppUserGroupId",
                table: "AppUserPoints",
                column: "AppUserGroupId",
                principalTable: "AppUserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
