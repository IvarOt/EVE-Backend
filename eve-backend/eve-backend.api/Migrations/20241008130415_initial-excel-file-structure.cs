using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eve_backend.api.Migrations
{
    /// <inheritdoc />
    public partial class initialexcelfilestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExcelObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcelFileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcelObjects_ExcelFiles_ExcelFileId",
                        column: x => x.ExcelFileId,
                        principalTable: "ExcelFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcelObjects_ExcelFileId",
                table: "ExcelObjects",
                column: "ExcelFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelObjects");

            migrationBuilder.DropTable(
                name: "ExcelFiles");
        }
    }
}
