using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class appuseroptionsmodified1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "AppUserOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserOptions_QuestionId",
                table: "AppUserOptions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserOptions_Questions_QuestionId",
                table: "AppUserOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserOptions_Questions_QuestionId",
                table: "AppUserOptions");

            migrationBuilder.DropIndex(
                name: "IX_AppUserOptions_QuestionId",
                table: "AppUserOptions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "AppUserOptions");
        }
    }
}
