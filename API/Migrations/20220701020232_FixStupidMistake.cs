using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FixStupidMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStudySession",
                table: "News");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStudySession",
                table: "Decks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4b74547-1385-47eb-80fa-1c29d573f571",
                column: "ConcurrencyStamp",
                value: "6ef1db44-edd5-4f6b-96cd-96f7948ca3d8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54fa3f74-3e4f-47bb-8bed-8a0a194bf5ca", "AQAAAAEAACcQAAAAEM7CITctZipx+pc4xFW6M7p6MoVPGR1k9/PglA73ARpaq9SHS8NQ5O383mha4/nfjA==", "1640cb9d-483a-4f84-a87f-df65d50ce0cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4adc59e7-3334-49bf-b860-a79e543cc925", "AQAAAAEAACcQAAAAEEiwhvV1VbOMvBQXzc6FXU5pl5uPkgLRta1hqSNdiA8y22ZjcuTTlcK4rlWfRTo+rw==", "7ff9d0f2-9310-46d8-9069-f80c91f1ddfd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7e71688-39f1-4809-831e-20a90de753b7", "AQAAAAEAACcQAAAAEMFwQuN6S8PkFjhJgHB+3K/KwTZbr2M3Lp5YkpCLT3fmFKu8DwStbPUwwNeUitjZEA==", "5d48614e-dab1-42a0-a3ce-bd60f5ac4465" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e43b8727-ab76-4d48-b4f8-7591572cb6e1", "AQAAAAEAACcQAAAAEMORTmF9/UdAi6tLQVehwNaSc43czPiqTCZAy6mDDBXJEs7d7sdrF4aJsMSLFBx/qw==", "91ca184b-146f-49bc-b3af-61dbb05fbb62" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75993074-eae1-4029-8592-04728f55dc35", "AQAAAAEAACcQAAAAEP1OS1y9RdCnE54ZYST95iHIbrYoQIme99BGcTzSzASC0FO/oDRtfKMSxxGlUgleTA==", "d18926cf-2bcc-430a-b484-84c642318e8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad195763-0f5b-4027-89fc-5892b66496c6", "AQAAAAEAACcQAAAAEIYtgwqMcAXgWgGe0TOCR1zJzAKi+BgnXqAzdy8gziI4s2lq15s04EEQaQln9xuk3g==", "49dcda3c-c006-44ed-9aba-f108c609a101" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6f7abcbc-d257-485f-a84d-fecc765bd6fe", "AQAAAAEAACcQAAAAEAHDT3QiDugCwv/oZjwKvrO78ACr7t0QjQfmm27WDwrVCCP1ypEFQ8PFMR6JgEZVrA==", "9d9e0da2-b415-40d0-8974-9a56803c6805" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "38e0cf45-530a-4cc3-9fd1-2b044b5392f7", "AQAAAAEAACcQAAAAEI6tno2NBkN/nqEs+p+Tm116kHElEpaMAUZcSLgiJNcHJmVasQBY9SXdvSqrQ7DmyQ==", "076a7b4a-f89e-4626-ad5d-c740483a4d9e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a2fde02-f5e5-41ea-9129-58048b4b532d", "AQAAAAEAACcQAAAAEDaNjE+lbHtAyMX6lGHCfMvQDOF9yFsbFWmz3ydS5SReqG76+Kk3WcZceWN+W7VQQA==", "e51aaaaf-22ee-4002-a24e-88ab7bbab767" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69c0eb2f-a198-4469-9a21-ea030a253d73", "AQAAAAEAACcQAAAAELxY1N41naD9egZQE4jdbeJJ7rgVFfUG1iHCZzh63aVWHSvT/q9uXrxKh2nqvyeeYw==", "bfa98824-1ea5-44c7-ba1c-a20bb7b4f56e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStudySession",
                table: "Decks");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStudySession",
                table: "News",
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
        }
    }
}
