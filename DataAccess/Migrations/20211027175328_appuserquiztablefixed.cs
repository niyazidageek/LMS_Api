using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class appuserquiztablefixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserQuiz_AspNetUsers_AppUserId",
                table: "AppUserQuiz");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserQuiz_Quizzes_QuizId",
                table: "AppUserQuiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserQuiz",
                table: "AppUserQuiz");

            migrationBuilder.RenameTable(
                name: "AppUserQuiz",
                newName: "AppUserQuizzes");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserQuiz_QuizId",
                table: "AppUserQuizzes",
                newName: "IX_AppUserQuizzes_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserQuiz_AppUserId",
                table: "AppUserQuizzes",
                newName: "IX_AppUserQuizzes_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserQuizzes",
                table: "AppUserQuizzes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserQuizzes_AspNetUsers_AppUserId",
                table: "AppUserQuizzes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserQuizzes_Quizzes_QuizId",
                table: "AppUserQuizzes",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserQuizzes_AspNetUsers_AppUserId",
                table: "AppUserQuizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserQuizzes_Quizzes_QuizId",
                table: "AppUserQuizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserQuizzes",
                table: "AppUserQuizzes");

            migrationBuilder.RenameTable(
                name: "AppUserQuizzes",
                newName: "AppUserQuiz");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserQuizzes_QuizId",
                table: "AppUserQuiz",
                newName: "IX_AppUserQuiz_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserQuizzes_AppUserId",
                table: "AppUserQuiz",
                newName: "IX_AppUserQuiz_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserQuiz",
                table: "AppUserQuiz",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserQuiz_AspNetUsers_AppUserId",
                table: "AppUserQuiz",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserQuiz_Quizzes_QuizId",
                table: "AppUserQuiz",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
