using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class appusergrouppointrelationfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroups_AppUserPoints_AppUserGroupPointId",
                table: "AppUserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPoints_AspNetUsers_AppUserId",
                table: "AppUserPoints");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPoints_AppUserId",
                table: "AppUserPoints");

            migrationBuilder.DropIndex(
                name: "IX_AppUserGroups_AppUserGroupPointId",
                table: "AppUserGroups");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AppUserPoints");

            migrationBuilder.DropColumn(
                name: "AppUserGroupPointId",
                table: "AppUserGroups");

            migrationBuilder.AddColumn<int>(
                name: "AppUserGroupId",
                table: "AppUserPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPoints_AppUserGroupId",
                table: "AppUserPoints",
                column: "AppUserGroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPoints_AppUserGroups_AppUserGroupId",
                table: "AppUserPoints",
                column: "AppUserGroupId",
                principalTable: "AppUserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPoints_AppUserGroups_AppUserGroupId",
                table: "AppUserPoints");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPoints_AppUserGroupId",
                table: "AppUserPoints");

            migrationBuilder.DropColumn(
                name: "AppUserGroupId",
                table: "AppUserPoints");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "AppUserPoints",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserGroupPointId",
                table: "AppUserGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPoints_AppUserId",
                table: "AppUserPoints",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserGroups_AppUserGroupPointId",
                table: "AppUserGroups",
                column: "AppUserGroupPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroups_AppUserPoints_AppUserGroupPointId",
                table: "AppUserGroups",
                column: "AppUserGroupPointId",
                principalTable: "AppUserPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPoints_AspNetUsers_AppUserId",
                table: "AppUserPoints",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
