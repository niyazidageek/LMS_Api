using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class groupmaxpointsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupMaxPointId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxGrade",
                table: "Assignments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "GroupMaxPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxPoint = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMaxPoints", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupMaxPointId",
                table: "Groups",
                column: "GroupMaxPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId",
                table: "Groups",
                column: "GroupMaxPointId",
                principalTable: "GroupMaxPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupMaxPoints");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupMaxPointId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupMaxPointId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "MaxGrade",
                table: "Assignments");
        }
    }
}
