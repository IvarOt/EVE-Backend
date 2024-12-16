using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eve_backend.api.Migrations
{
    /// <inheritdoc />
    public partial class identifierInObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "ExcelObjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "ExcelObjects");
        }
    }
}
