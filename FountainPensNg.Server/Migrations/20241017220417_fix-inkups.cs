using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class fixinkups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FountainPens_Inks_CurrentInkId",
                table: "FountainPens");

            migrationBuilder.DropIndex(
                name: "IX_FountainPens_CurrentInkId",
                table: "FountainPens");

            migrationBuilder.DropColumn(
                name: "CurrentInkId",
                table: "FountainPens");

            migrationBuilder.DropColumn(
                name: "CurrentInkRating",
                table: "FountainPens");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "InkedUps",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "InkedUps");

            migrationBuilder.AddColumn<int>(
                name: "CurrentInkId",
                table: "FountainPens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentInkRating",
                table: "FountainPens",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FountainPens_CurrentInkId",
                table: "FountainPens",
                column: "CurrentInkId");

            migrationBuilder.AddForeignKey(
                name: "FK_FountainPens_Inks_CurrentInkId",
                table: "FountainPens",
                column: "CurrentInkId",
                principalTable: "Inks",
                principalColumn: "Id");
        }
    }
}
