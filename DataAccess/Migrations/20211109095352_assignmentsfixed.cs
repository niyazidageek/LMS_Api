using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class assignmentsfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LessonAssignments_LessonId",
                table: "LessonAssignments",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonAssignments_Lessons_LessonId",
                table: "LessonAssignments",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonAssignments_Lessons_LessonId",
                table: "LessonAssignments");

            migrationBuilder.DropIndex(
                name: "IX_LessonAssignments_LessonId",
                table: "LessonAssignments");
        }
    }
}
