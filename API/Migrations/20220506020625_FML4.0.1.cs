using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FML401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "04192af3-78cc-4cb5-a838-e01bb1f22767", "374e233b-65ee-45c0-b69b-4fbefcd3f8f8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "38d1410b-ad63-474f-96b8-2b95f1586b59", "5e32d808-771b-4687-ac2a-ea976567280e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a06b69b2-86a0-4ad8-969c-276809b67bb1", "58829e53-c191-402b-97b6-2bb9f4f52607" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "78c0e261-7786-4ccf-95ef-36a8083cdea4", "e7e64430-6774-4d21-81e4-91a289db47e1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "af1e33eb-7bc4-4ca1-a7aa-e2cb2c503a03", "f6668ecf-6113-493c-9e74-75b277d8fb19" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0b2a1194-db59-4acf-b61d-14e89ef4b238", "6beda073-eb2c-4e43-b734-b0c67123ba19" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1309e4cd-18ce-4aef-be99-66cb69a57cc7", "2b65d32c-0c2a-4aa4-b226-4ea9edb79c2b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1fc82f8d-e286-4978-b51d-3568794a1d61", "09c773a2-8d31-4778-b7f4-4223c8a9232f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "61c7a2bd-1196-445d-a52c-9db65157353a", "2ca3f4fa-7e2d-4a34-9278-79c84be0121b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "987741e5-6981-4e3c-aff3-b0cbd6109512", "fd437364-3a0c-4d31-b297-1df5fa4b3463" });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "FlashcardID", "Answer", "BackContentLayout", "Content1", "Content2", "Content3", "Content4", "Content5", "Content6", "CreationDate", "DeckId", "DueDate", "FrontContentLayout", "LastUpdated", "Level" },
                values: new object[] { new Guid("aa75d1eb-0742-4e82-b322-5f95ebd9f58e"), "kill", 0, "Exercitation nostrud proident irure ut voluptate qui. Officia incididunt sint culpa irure do amet culpa occaecat adipisicing. Ipsum proident et ad deserunt pariatur elit dolore esse. Consectetur cupidatat laboris qui quis tempor ipsum. Esse duis incididunt labore. Mollit officia aliquip Lorem consequat dolor tempor aliquip laboris.", "In non veniam officia mollit ea consequat reprehenderit excepteur ipsum ex non. Non fugiat labore reprehenderit est in commodo excepteur cillum laborum aliqua. Enim proident nisi ex labore enim velit velit ullamco anim magna. Esse consectetur nostrud eiusmod eu Lorem laborum esse minim. Officia elit minim aliqua velit in. Esse duis labore sunt sint fugiat amet culpa exercitation sint qui exercitation esse.", "", "https://audio12.forvo.com/audios/mp3/6/o/6o_9014612_41_617305_1.mp3", "", "", new DateTime(2020, 1, 2, 19, 31, 19, 0, DateTimeKind.Utc), new Guid("18d5f976-3c81-1711-ccd3-1ba2e7caa24c"), new DateTime(2020, 3, 23, 19, 31, 19, 0, DateTimeKind.Utc), "HORIZONTAL_SPLIT", new DateTime(2020, 1, 2, 19, 31, 19, 0, DateTimeKind.Utc), 9 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("aa75d1eb-0742-4e82-b322-5f95ebd9f58e"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4a0c927c-3a2b-4ef9-b916-9e98f8585077", "6005a7bc-828f-4bf0-8d46-1abf8b52cc32" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "01b6f72b-b3d7-4ba4-8dd3-2bbcaca2d4f3", "ddad2ab2-4d6b-44b5-8126-5febdf56e64c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1c9274e9-5275-403e-8c8d-28f0eadbfe67", "9bfa3d3a-890b-4ad5-a06d-bf54f4a8466e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0bad3109-7202-4cda-94a5-a1793de37316", "378495e6-26a8-4604-ac0e-a1c93b0dbcf6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "40f6fe8c-dec8-4b56-b448-b53e987ee633", "695dd77e-6b1a-42f3-9b89-66a45da12207" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "516c5f67-1c35-485b-9a5c-b00110463861", "a9c3e9da-c1a1-4ad9-b036-cd045209b24d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ad078cbb-eec6-4200-bd27-9460e0a25aa9", "86125193-8133-4bce-99fd-1747c1d638cd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6ad2e22c-a5ed-4436-80a4-64bcd92fb11d", "405ffa85-2feb-450a-85d5-f41df54a54ef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dc2800b0-670d-4431-918f-12904eb8f1e5", "c1ccea4f-6391-4973-bb0e-30dc29041a00" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fab7e24c-1d62-4ca7-a5d0-a2d5a840780f", "c8873976-de32-4460-9ccd-d36fa6d450f8" });
        }
    }
}
