using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FountainPens",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FountainPens",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Inks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Inks",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "FountainPens",
                newName: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "Nib",
                table: "FountainPens",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Inks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "FountainPens",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "Nib",
                table: "FountainPens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "FountainPens",
                columns: new[] { "Id", "Color", "Comment", "CurrentInkId", "Maker", "ModelName", "Nib", "Photo", "Status" },
                values: new object[,]
                {
                    { 1, "", "Nice writer, dries out quickly?", null, "Jinhao", "X159", 2, "", 0 },
                    { 2, "", "Nice writer, dries out quickly?", null, "Jinhao", "X159", 2, "", 0 }
                });

            migrationBuilder.InsertData(
                table: "Inks",
                columns: new[] { "Id", "Color", "Color_CIELAB_L", "Color_CIELAB_a", "Color_CIELAB_b", "Comment", "InkName", "Maker", "Photo", "Status" },
                values: new object[] { 1, "", null, null, null, "striking blue", "Iroshizuku Kon-Peki", "Pilot", "", 0 });
        }
    }
}
