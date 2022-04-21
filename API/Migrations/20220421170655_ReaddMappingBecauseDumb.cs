using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class ReaddMappingBecauseDumb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Decks_DeckID",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "DeckID",
                table: "Flashcards",
                newName: "DeckId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_DeckID",
                table: "Flashcards",
                newName: "IX_Flashcards_DeckId");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeckId",
                table: "Flashcards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "DeckID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "DeckId",
                table: "Flashcards",
                newName: "DeckID");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_DeckId",
                table: "Flashcards",
                newName: "IX_Flashcards_DeckID");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeckID",
                table: "Flashcards",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Decks_DeckID",
                table: "Flashcards",
                column: "DeckID",
                principalTable: "Decks",
                principalColumn: "DeckID");
        }
    }
}
