using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eve_backend.api.Migrations
{
    /// <inheritdoc />
    public partial class Ondeletecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcelObjects_ExcelFiles_ExcelFileId",
                table: "ExcelObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ExcelProperties_ExcelObjects_ExcelObjectId",
                table: "ExcelProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcelObjects_ExcelFiles_ExcelFileId",
                table: "ExcelObjects",
                column: "ExcelFileId",
                principalTable: "ExcelFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExcelProperties_ExcelObjects_ExcelObjectId",
                table: "ExcelProperties",
                column: "ExcelObjectId",
                principalTable: "ExcelObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcelObjects_ExcelFiles_ExcelFileId",
                table: "ExcelObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ExcelProperties_ExcelObjects_ExcelObjectId",
                table: "ExcelProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcelObjects_ExcelFiles_ExcelFileId",
                table: "ExcelObjects",
                column: "ExcelFileId",
                principalTable: "ExcelFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcelProperties_ExcelObjects_ExcelObjectId",
                table: "ExcelProperties",
                column: "ExcelObjectId",
                principalTable: "ExcelObjects",
                principalColumn: "Id");
        }
    }
}
