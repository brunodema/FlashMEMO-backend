using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddFlashcardProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentLayout",
                table: "Flashcards",
                newName: "FrontContentLayout");

            migrationBuilder.AddColumn<int>(
                name: "BackContentLayout",
                table: "Flashcards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Content4",
                table: "Flashcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content5",
                table: "Flashcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content6",
                table: "Flashcards",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackContentLayout",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Content4",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Content5",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Content6",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "FrontContentLayout",
                table: "Flashcards",
                newName: "ContentLayout");
        }
    }
}
