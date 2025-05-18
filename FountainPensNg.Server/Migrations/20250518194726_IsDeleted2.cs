using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FountainPensNg.Server.Migrations
{
    /// <inheritdoc />
    public partial class IsDeleted2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertedAt",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "InsertedAt",
                table: "Inks");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Inks");

            migrationBuilder.DropColumn(
                name: "InsertedAt",
                table: "FountainPens");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "FountainPens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedAt",
                table: "Papers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Papers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedAt",
                table: "Inks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Inks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedAt",
                table: "FountainPens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "FountainPens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
