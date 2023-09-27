using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortenerApi.Migrations
{
    public partial class AddNewColumnMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "UrlTables",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "UrlTables",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_UrlTables_ShortUrl",
                table: "UrlTables",
                column: "ShortUrl",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UrlTables_ShortUrl",
                table: "UrlTables");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "UrlTables");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "UrlTables");
        }
    }
}
