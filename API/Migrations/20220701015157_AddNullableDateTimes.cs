using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddNullableDateTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_OwnerId",
                table: "News");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStudySession",
                table: "News",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4b74547-1385-47eb-80fa-1c29d573f571",
                column: "ConcurrencyStamp",
                value: "45a4d53d-636f-4676-845a-6dacf2a74cd8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "334c434b-9612-4417-bc5e-43646a905396", "AQAAAAEAACcQAAAAEE552YxyrOO+LEskZVrUTnyYW17egJMAKwu92fDrsfxCdxDTpgSYWdTMHQUgYbqRBA==", "e1d3d856-3042-4b28-87dc-96c0c67adf05" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "06eb83ca-5c3a-4729-bd45-822fac532ea9", "AQAAAAEAACcQAAAAECLhcRJyBXK6S/L/OAvUts476Gz5QNd+HkXKIbYXnkcLpgfynyOTZo8//h9uaxgEeA==", "01c6952d-3c84-4053-bd5c-fdc2401aaa08" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d7cd90a-2e6e-45ca-adc1-81ef56578726", "AQAAAAEAACcQAAAAEET4JRpvjEb8yxN2Od3+rDryT5XAjwB+Wa5LSJ+2XD38oZxyMk/UCbMmRsaSWzhXPw==", "5dc1fa8a-64ac-4e30-86e0-0e93c5a7704f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0ba35417-46ea-43e8-b525-2b4cd751276f", "AQAAAAEAACcQAAAAEDQL5xqj65O1YpbDQk/YVi4sz1tRSARS5iRZBYbKbESC+UKXR4xHpL3r6oO3B5pECw==", "cd11e556-41f2-424a-9b8c-38a94ec043e9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bc45bfdf-e80a-4623-b25c-5d26c80fa354", "AQAAAAEAACcQAAAAEIWrCE/Sb9XhrQLUcgglLiII17aAZy+mjtaixRqobZqS9XajuQTn96Z7vTAJixlayg==", "302e5b00-4a35-45fd-bc1b-d2e23cfe39f8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "806b51a0-7037-4365-8063-bf4ddf936494", "AQAAAAEAACcQAAAAEKLj+7fAXKifrmCAY+7xuaf/oM1M5AU5VUVI1IYWiY4Z15K7mmL8zkOuue5LphsDWA==", "bf26d9f8-aefc-432a-b814-32dfdc0c3f53" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e178aa0d-000b-4937-bf66-aa0e82666deb", "AQAAAAEAACcQAAAAEOKmR4/o+YHrShvdAAnFuu6H4zmq0P1TaXGH2xqJGMcPMJorAIJ6diKVj99XDlunbA==", "a99f86f8-df5a-40ec-bfd8-e876d2ba9aea" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5f132d3b-cad6-4ecc-90a8-29402b52eee7", "AQAAAAEAACcQAAAAEHzt+vfAsilTupxyV9PUg1Qy/sOtcoyE+0VZkWp3h5pmWw2XoQmZuVh5Ro4UWE/0kQ==", "2495dfa2-51fd-4522-a9c3-5bc1cf2a9a10" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dc49b610-0996-404e-98f8-be5dda674058", "AQAAAAEAACcQAAAAEOXPLi6CSChuG2a5JPRkmi/CbNyq8L186HozNrGLg3pjOll3KleOlu42KvnZVjSR9Q==", "21513f45-0b08-45e7-8fb3-6647e78f1ce7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "50d7fe01-c557-4d8d-91cc-6422e35c709a", "AQAAAAEAACcQAAAAEOgS1scbCIF0EanwoE1gj4PtzPv7MCp9Ba8Yy72MmuNpW80nGaHq8/BZ4JOEbIFRgQ==", "508f7775-6e92-4ac7-842c-9548d95d3028" });

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId",
                table: "Decks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_OwnerId",
                table: "News",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_OwnerId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "LastStudySession",
                table: "News");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4b74547-1385-47eb-80fa-1c29d573f571",
                column: "ConcurrencyStamp",
                value: "9b33dfde-20a7-466a-8390-92ab2cd3d9cf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a7f087c-7416-41d5-bea0-5bcdd148d44c", "AQAAAAEAACcQAAAAEAN3mHd0Zla3zpgdTOyw7znAuSJD08vtdQ2RTo4jAkcRz2YotCmtv8WBly/knRiypw==", "c8238f0e-2f54-44fa-9478-a9ce5492b869" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "adfaced8-5d8e-41ba-8747-6bdf4e729cea", "AQAAAAEAACcQAAAAEFe6D1ObPf8DEus5Oy4v4TSzsDrvTPpfym9Bh2yc/AEaQ8rGvNHKOAhePWf4JVUEnQ==", "e92da4b6-264d-425b-8a1b-70b5b342bbff" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0993612-9c06-40c4-8e16-14d8a112dd8c", "AQAAAAEAACcQAAAAEKvCUwLynX6h8t8UnuPaaKaoLeXURfM7WocBqHTk6wGAl8+B1kOhuw3GBatdW2nCIg==", "2e9d1343-3c9b-4626-bf3c-b8386132c90f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55432d2e-8e5d-4a8d-873e-5c7cd2aa05db", "AQAAAAEAACcQAAAAEJjjqr1UpTFXOuEMLbdGZkcPQjCE2xvL1PyvFo4nluAPtbZiAAjU+B7XDNcZh/qClw==", "ceefddb4-b176-456d-a7c5-ee1e81ca9bd3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a64f8c7-521c-469d-ae26-039be3a35561", "AQAAAAEAACcQAAAAELl08+ynFHuC8I8xP5yinh7D6rEJGLiDuJWfHT0OoR+OrigLuYHpr22IbkLU2TleRg==", "f0f6e2fc-bcc2-438f-9e39-ece4b7508193" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d29b6230-525a-4db9-9c34-63f49c8ecb56", "AQAAAAEAACcQAAAAEDCHqP6bEh+BaSMJD4l0j/z0vVSHCMIvTRfFtz9eqO3R9BHMRpn6TtRqVPCpnFaCDg==", "b92e35e0-8de3-4c2d-bbd0-6af66c808594" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80f0ae4e-0e2f-4821-9b22-84538a5c6d13", "AQAAAAEAACcQAAAAEBXirMXbFP9fO+KE57XPI7w0+qcd1OSh4pGZW8VSY710sBAYCDFDdYGBVPQ63dmFvw==", "83529c38-9bb6-42bd-8800-1898113b3f21" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee5cd150-7ec1-44f1-9d2a-d51c0d49099f", "AQAAAAEAACcQAAAAEFdtjWwKTd/l33tQKv0eQfxblNiGlFSn8cNc8UVNkt7WqlYVsDxBY/pyVBqFtgHUiw==", "c173aeed-d6e0-4339-960d-01fe1a621290" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8fe8f25-0086-4b12-b894-147b98c352fc", "AQAAAAEAACcQAAAAEEU98R3nenw0eo9s4TNRU+2Icuf+fUM1R6C/rjwlni4b5GqBRNt19EngtsjMmH4E3g==", "e73a1bdb-e040-4229-b177-f98ce11b7911" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ec89ee6-77b8-47f7-bd91-4084c0cd57a0", "AQAAAAEAACcQAAAAEKPKoIKUUSGAJtTs2MWgU1G2PQrSs2jDsXLlvLt0vA/ggb0mVkLFLM9lbzvpwXsraQ==", "2eeaca8d-4f62-4210-b532-d6fd364d42d2" });

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_AspNetUsers_OwnerId",
                table: "Decks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_OwnerId",
                table: "News",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
