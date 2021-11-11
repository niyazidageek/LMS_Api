using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class groupmaxpointfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId",
                table: "Groups");

            migrationBuilder.AlterColumn<int>(
                name: "GroupMaxPointId",
                table: "Groups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId",
                table: "Groups",
                column: "GroupMaxPointId",
                principalTable: "GroupMaxPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupMaxPoints_GroupMaxPointId",
                table: "Groups");

            migrationBuilder.AlterColumn<int>(
                name: "GroupMaxPointId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
