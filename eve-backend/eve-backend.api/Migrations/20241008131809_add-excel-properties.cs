using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eve_backend.api.Migrations
{
    /// <inheritdoc />
    public partial class addexcelproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExcelObjects");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ExcelObjects");

            migrationBuilder.CreateTable(
                name: "ExcelProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcelObjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcelProperties_ExcelObjects_ExcelObjectId",
                        column: x => x.ExcelObjectId,
                        principalTable: "ExcelObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcelProperties_ExcelObjectId",
                table: "ExcelProperties",
                column: "ExcelObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelProperties");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExcelObjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ExcelObjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
