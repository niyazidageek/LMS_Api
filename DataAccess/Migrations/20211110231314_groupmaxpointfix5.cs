using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class groupmaxpointfix5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupMaxPointId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupMaxPointId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupMaxPointId1",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMaxPoints_GroupId",
                table: "GroupMaxPoints",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMaxPoints_Groups_GroupId",
                table: "GroupMaxPoints",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMaxPoints_Groups_GroupId",
                table: "GroupMaxPoints");

            migrationBuilder.DropIndex(
                name: "IX_GroupMaxPoints_GroupId",
                table: "GroupMaxPoints");

            migrationBuilder.AddColumn<int>(
                name: "GroupMaxPointId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupMaxPointId1",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupMaxPointId1",
                table: "Groups",
                column: "GroupMaxPointId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId1",
                table: "Groups",
                column: "GroupMaxPointId1",
                principalTable: "GroupMaxPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
