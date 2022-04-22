using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class ChangeLanguagePK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Decks_LanguageId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "DbId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Decks");

            migrationBuilder.AlterColumn<string>(
                name: "ISOCode",
                table: "Languages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageISOCode",
                table: "Decks",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "ISOCode");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_LanguageISOCode",
                table: "Decks",
                column: "LanguageISOCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Languages_LanguageISOCode",
                table: "Decks",
                column: "LanguageISOCode",
                principalTable: "Languages",
                principalColumn: "ISOCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Languages_LanguageISOCode",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Decks_LanguageISOCode",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "LanguageISOCode",
                table: "Decks");

            migrationBuilder.AlterColumn<string>(
                name: "ISOCode",
                table: "Languages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Languages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DbId",
                table: "Languages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Decks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_LanguageId",
                table: "Decks",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Languages_LanguageId",
                table: "Decks",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
