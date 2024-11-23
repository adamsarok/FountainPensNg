using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class fulltext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Paper",
                table: "Paper");

            migrationBuilder.RenameTable(
                name: "Paper",
                newName: "Papers");

            migrationBuilder.RenameIndex(
                name: "IX_Paper_FullText",
                table: "Papers",
                newName: "IX_Papers_FullText");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Papers",
                table: "Papers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Papers",
                table: "Papers");

            migrationBuilder.RenameTable(
                name: "Papers",
                newName: "Paper");

            migrationBuilder.RenameIndex(
                name: "IX_Papers_FullText",
                table: "Paper",
                newName: "IX_Paper_FullText");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paper",
                table: "Paper",
                column: "Id");
        }
    }
}
