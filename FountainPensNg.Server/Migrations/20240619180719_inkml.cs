using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class inkml : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ml",
                table: "Inks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ml",
                table: "Inks");
        }
    }
}
