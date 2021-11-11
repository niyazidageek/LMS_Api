using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class groupmaxpointfix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupMaxPointId",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "GroupMaxPointId1",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "GroupMaxPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupMaxPointId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupMaxPointId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupMaxPoints");

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
    }
}
