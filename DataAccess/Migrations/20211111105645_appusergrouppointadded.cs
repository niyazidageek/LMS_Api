using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class appusergrouppointadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserGroupPointId",
                table: "AppUserGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppUserPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Point = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserPoints_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserGroups_AppUserGroupPointId",
                table: "AppUserGroups",
                column: "AppUserGroupPointId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPoints_AppUserId",
                table: "AppUserPoints",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserGroups_AppUserPoints_AppUserGroupPointId",
                table: "AppUserGroups",
                column: "AppUserGroupPointId",
                principalTable: "AppUserPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserGroups_AppUserPoints_AppUserGroupPointId",
                table: "AppUserGroups");

            migrationBuilder.DropTable(
                name: "AppUserPoints");

            migrationBuilder.DropIndex(
                name: "IX_AppUserGroups_AppUserGroupPointId",
                table: "AppUserGroups");

            migrationBuilder.DropColumn(
                name: "AppUserGroupPointId",
                table: "AppUserGroups");
        }
    }
}
