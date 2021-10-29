using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class relationsbetweenfileandquestionfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Materials_MaterialId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Materials_MaterialId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_MaterialId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Options_MaterialId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Options");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Options",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Options");

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Options",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_MaterialId",
                table: "Questions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_MaterialId",
                table: "Options",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Materials_MaterialId",
                table: "Options",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Materials_MaterialId",
                table: "Questions",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
