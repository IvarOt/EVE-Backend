using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eve_backend.api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObjectStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectStructure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExcelFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StructureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcelFiles_ObjectStructure_Id",
                        column: x => x.Id,
                        principalTable: "ObjectStructure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExcelObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExcelFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcelObjects_ExcelFiles_ExcelFileId",
                        column: x => x.ExcelFileId,
                        principalTable: "ExcelFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExcelProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcelObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcelProperties_ExcelObjects_ExcelObjectId",
                        column: x => x.ExcelObjectId,
                        principalTable: "ExcelObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcelObjects_ExcelFileId",
                table: "ExcelObjects",
                column: "ExcelFileId");

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

            migrationBuilder.DropTable(
                name: "ExcelObjects");

            migrationBuilder.DropTable(
                name: "ExcelFiles");

            migrationBuilder.DropTable(
                name: "ObjectStructure");
        }
    }
}
