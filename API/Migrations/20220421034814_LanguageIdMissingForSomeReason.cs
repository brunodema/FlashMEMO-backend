using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class LanguageIdMissingForSomeReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Decks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_LanguageId",
                table: "Decks",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "IX_Decks_LanguageId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Decks");
        }
    }
}
