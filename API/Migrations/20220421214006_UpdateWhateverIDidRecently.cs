using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateWhateverIDidRecently : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "IX_Decks_OwnerId",
                table: "Decks");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Decks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LanguageId",
                table: "Decks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId1",
                table: "Decks",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_OwnerId1",
                table: "Decks",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId1",
                table: "Decks",
                column: "OwnerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId1",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "IX_Decks_OwnerId1",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "Decks");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Decks",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LanguageId",
                table: "Decks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_OwnerId",
                table: "Decks",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId",
                table: "Decks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId");
        }
    }
}
