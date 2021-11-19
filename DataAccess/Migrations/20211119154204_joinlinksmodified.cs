using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class joinlinksmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinLink",
                table: "Lessons");

            migrationBuilder.CreateTable(
                name: "LessonJoinLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    JoinLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonJoinLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonJoinLinks_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonJoinLinks_LessonId",
                table: "LessonJoinLinks",
                column: "LessonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonJoinLinks");

            migrationBuilder.AddColumn<string>(
                name: "JoinLink",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
