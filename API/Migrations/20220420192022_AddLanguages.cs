using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddLanguages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FrontContent",
                table: "Flashcards",
                newName: "Content3");

            migrationBuilder.RenameColumn(
                name: "BackContent",
                table: "Flashcards",
                newName: "Content2");

            migrationBuilder.AddColumn<string>(
                name: "Content1",
                table: "Flashcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContentLayout",
                table: "Flashcards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    DbId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ISOCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropColumn(
                name: "Content1",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "ContentLayout",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "Content3",
                table: "Flashcards",
                newName: "FrontContent");

            migrationBuilder.RenameColumn(
                name: "Content2",
                table: "Flashcards",
                newName: "BackContent");
        }
    }
}
