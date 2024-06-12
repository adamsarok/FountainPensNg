using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Maker = table.Column<string>(type: "text", nullable: false),
                    InkName = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Color_CIELAB_L = table.Column<double>(type: "double precision", nullable: true),
                    Color_CIELAB_a = table.Column<double>(type: "double precision", nullable: true),
                    Color_CIELAB_b = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FountainPens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Maker = table.Column<string>(type: "text", nullable: false),
                    ModelName = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Nib = table.Column<int>(type: "integer", nullable: false),
                    CurrentInkId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FountainPens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FountainPens_Inks_CurrentInkId",
                        column: x => x.CurrentInkId,
                        principalTable: "Inks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InkedUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    MatchRating = table.Column<int>(type: "integer", nullable: false),
                    FountainPenId = table.Column<int>(type: "integer", nullable: false),
                    InkId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InkedUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InkedUps_FountainPens_FountainPenId",
                        column: x => x.FountainPenId,
                        principalTable: "FountainPens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InkedUps_Inks_InkId",
                        column: x => x.InkId,
                        principalTable: "Inks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_FountainPens_CurrentInkId",
                table: "FountainPens",
                column: "CurrentInkId");

            migrationBuilder.CreateIndex(
                name: "IX_InkedUps_FountainPenId",
                table: "InkedUps",
                column: "FountainPenId");

            migrationBuilder.CreateIndex(
                name: "IX_InkedUps_InkId",
                table: "InkedUps",
                column: "InkId");

            migrationBuilder.CreateIndex(
                name: "IX_Inks_Maker_InkName",
                table: "Inks",
                columns: new[] { "Maker", "InkName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InkedUps");

            migrationBuilder.DropTable(
                name: "FountainPens");

            migrationBuilder.DropTable(
                name: "Inks");
        }
    }
}
