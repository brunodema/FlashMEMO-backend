using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FML404 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a3a4d11f-72b1-4602-8f69-89553e7ff16e", "5257366a-b27f-455c-899d-c93b9a89ef57" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b56f12a3-8f25-46fd-9907-0c31b1f4a22c", "710e6332-f072-436c-9251-3f9d25027d29" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "54820e0c-0cc0-4c0d-bcb9-bbd0293eb07d", "18402e19-2abd-41aa-a576-55cb0a33c302" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "26980167-9147-4080-89b1-d6eb13ee9493", "ddfa1dbf-47f9-4635-9c8a-56c06369a1d8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d8c01bf4-cd84-4860-bf96-2f946466e864", "71e2b1b8-05f7-46aa-856d-0a4d64f79811" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "03409c14-0bf2-45a3-8dd7-9e5b46297e93", "e170194e-e4d6-4de2-8bbe-3b6a174fd6be" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4d169f31-1956-4759-84ff-27ed1e83c32c", "5a8f29f6-8aae-4c36-b346-03de13da860d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c9fe595d-2311-4fab-8f52-1b4cca38f4b6", "80c03e13-c9e3-4b42-b5e8-63c4bf4c0bec" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c8ba0026-3ca2-4235-974f-4460ef85f9aa", "658d7ac5-500e-49cb-aa1f-c0b58dd5e797" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "63a87b13-ffdd-4c5c-9f37-c2c8d3989e95", "0e7216a9-0105-4fff-b548-f911b5fef7b5" });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "FlashcardID", "Answer", "BackContentLayout", "Content1", "Content2", "Content3", "Content4", "Content5", "Content6", "CreationDate", "DeckId", "DueDate", "FrontContentLayout", "LastUpdated", "Level" },
                values: new object[,]
                {
                    { new Guid("52588d3d-684e-44dc-a06c-4846ab19d3ff"), "Gelsenkirchen", 0, "Pariatur deserunt occaecat ad. Quis consequat voluptate excepteur voluptate laborum reprehenderit cupidatat sit proident est ex ad ad. Et anim nisi aute est amet ipsum cupidatat sunt adipisicing est nisi fugiat. Amet quis qui aliquip.", "<p><b>car</b></p><p>Category: Noun</p><p>Spelling: kɑː</p><p>Definitions:</p><ul><li>a four-wheeled road vehicle that is powered by an engine and is able to carry a small number of people</li></ul><p>Examples:</p><ul><li>she drove up in a car</li><li>we're going by car</li></ul>", "Pariatur enim proident sunt nostrud magna tempor qui incididunt dolor occaecat commodo consequat culpa velit. Nulla nostrud ipsum culpa do ex in sit aute consequat laborum eiusmod in dolor. Excepteur eiusmod aute ea aute est elit consequat. Proident sunt fugiat culpa do dolor fugiat tempor eiusmod officia eiusmod deserunt.", "Consectetur pariatur amet duis ullamco duis. Tempor aute veniam voluptate non anim ad aute esse qui minim ullamco adipisicing laborum amet mollit. Nisi esse ullamco cupidatat pariatur ex elit labore et officia ut. Ex laborum deserunt proident nostrud consequat laboris irure ad amet duis et. Cillum elit dolore velit mollit deserunt.", "", "", new DateTime(2020, 1, 24, 3, 10, 57, 0, DateTimeKind.Utc), new Guid("ad56c6b4-4791-7982-4ac7-cca45243607e"), new DateTime(2020, 2, 1, 9, 55, 41, 0, DateTimeKind.Utc), "TRIPLE_BLOCK", new DateTime(2020, 1, 28, 9, 55, 41, 0, DateTimeKind.Utc), 2 },
                    { new Guid("6ce50229-1b3b-4abc-ad1a-baba95efa2a2"), "", 0, "https://m.media-amazon.com/images/I/81rz6kuxieL._AC_SL1500_.jpg", "", "", "Enim eiusmod do velit est amet exercitation. Nostrud quis anim elit Lorem exercitation aliqua. Elit anim ad laboris enim esse non laboris amet labore commodo id proident nostrud incididunt. Sit do veniam eiusmod elit commodo et dolore sunt qui consectetur ut. Officia duis et laboris sit excepteur id nulla officia sit dolor mollit exercitation minim. Eu aliqua occaecat magna laboris ipsum culpa. Non irure minim quis ut. Do aliquip cupidatat excepteur sit enim anim ullamco aute dolore cupidatat.", "", "", new DateTime(2020, 1, 30, 10, 5, 32, 0, DateTimeKind.Utc), new Guid("18d5f976-3c81-1711-ccd3-1ba2e7caa24c"), new DateTime(2020, 5, 10, 15, 12, 11, 0, DateTimeKind.Utc), "SINGLE_BLOCK", new DateTime(2020, 1, 31, 15, 12, 11, 0, DateTimeKind.Utc), 10 },
                    { new Guid("c1adced1-c7a3-44ce-9fd2-5dc8f8e5f2f6"), "", 3, "<p><b>car</b></p><p>Category: Noun</p><p>Spelling: kɑː</p><p>Definitions:</p><ul><li>a four-wheeled road vehicle that is powered by an engine and is able to carry a small number of people</li></ul><p>Examples:</p><ul><li>she drove up in a car</li><li>we're going by car</li></ul>", "Irure id in laboris reprehenderit aliquip ea quis. Ipsum dolor enim quis non magna sint. Proident cupidatat anim amet culpa sunt eiusmod voluptate ipsum exercitation ad nostrud nostrud. Deserunt amet proident sit nisi ut. Esse sint ea consequat anim. Qui enim minim fugiat veniam labore irure ex. Anim nulla laborum mollit reprehenderit non cillum reprehenderit magna nulla aute quis.", "", "https://audio12.forvo.com/audios/mp3/u/n/un_9523171_118_7665857.mp3", "Incididunt ullamco consectetur ad in laboris duis irure eu ea anim sit est. Deserunt nisi velit consectetur id esse ipsum. Proident minim culpa esse occaecat sunt reprehenderit minim ex sint aliqua labore mollit non do. Sit anim exercitation laborum dolor aliquip. Tempor esse est sint non velit. Consectetur excepteur nulla quis laboris officia duis aute deserunt nostrud.", "", new DateTime(2020, 1, 4, 10, 20, 37, 0, DateTimeKind.Utc), new Guid("2a4424cc-2c1d-1309-a647-be27769a91c9"), new DateTime(2020, 4, 13, 10, 20, 37, 0, DateTimeKind.Utc), "VERTICAL_SPLIT", new DateTime(2020, 1, 4, 10, 20, 37, 0, DateTimeKind.Utc), 10 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("52588d3d-684e-44dc-a06c-4846ab19d3ff"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("6ce50229-1b3b-4abc-ad1a-baba95efa2a2"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("c1adced1-c7a3-44ce-9fd2-5dc8f8e5f2f6"));

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
        }
    }
}
