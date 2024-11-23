using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class papers_fulltext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "FullText",
                table: "Inks",
                type: "tsvector",
                nullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Maker", "InkName", "Comment", "Rating" });

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "FullText",
                table: "FountainPens",
                type: "tsvector",
                nullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Maker", "ModelName", "Comment", "Rating" });

            migrationBuilder.CreateTable(
                name: "Paper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Maker = table.Column<string>(type: "text", nullable: false),
                    PaperName = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ImageObjectKey = table.Column<string>(type: "text", nullable: false),
                    FullText = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true)
                        .Annotation("Npgsql:TsVectorConfig", "english")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Maker", "PaperName", "Comment", "Rating" }),
                    InsertedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paper", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inks_FullText",
                table: "Inks",
                column: "FullText")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_FountainPens_FullText",
                table: "FountainPens",
                column: "FullText")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Paper_FullText",
                table: "Paper",
                column: "FullText")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paper");

            migrationBuilder.DropIndex(
                name: "IX_Inks_FullText",
                table: "Inks");

            migrationBuilder.DropIndex(
                name: "IX_FountainPens_FullText",
                table: "FountainPens");

            migrationBuilder.DropColumn(
                name: "FullText",
                table: "Inks");

            migrationBuilder.DropColumn(
                name: "FullText",
                table: "FountainPens");
        }
    }
}
