using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateFuckingWordCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsID",
                table: "News",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "DeckID",
                table: "Decks",
                newName: "DeckId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "371c7ebb-208a-4a71-b5e9-2e6d6bd5ddcd", "692ca52d-890d-4224-beba-aea70c6ac04b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5da9d945-1656-4947-8ce9-9ca57610fd7a", "2e35a559-e295-4dab-92f2-3b7ede72d974" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7403721a-826a-4147-9587-8b7b0c4075db", "cb292d0f-7b84-4334-b4aa-4f0f8816bbf1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a3d9f178-961d-47d5-ae08-ebeb4fb00bb5", "d7d62a8a-a648-4532-98ee-602d8060582d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "49618954-1b5a-4c91-9462-0a8586ffeca9", "5b5e7639-5146-4564-b46d-ab0f5a69c691" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2681f031-6868-468c-a5c7-09bd238217d7", "5c6aeb42-bd6e-4743-a147-5ea8c1fd34ed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3bcc584b-379e-44e6-882d-aa3153484562", "15435d5d-223a-417c-ac3a-88a572abf5dc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "499745b9-a586-4e4c-a1c7-b493a32d27c1", "8cfee099-a0f5-41bb-b2ef-1561956a85df" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "92438338-669c-4ca5-8e32-131d9f97c9b9", "f3c36483-486a-441e-990a-3159fdec4bc3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf1f9693-9144-475d-98ec-47a046fa10c7", "87f58456-86ff-42c4-9752-032c4387576f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "News",
                newName: "NewsID");

            migrationBuilder.RenameColumn(
                name: "DeckId",
                table: "Decks",
                newName: "DeckID");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "743ad148-cfd1-4809-8c68-9de655fbb469", "ffc62067-27ae-4aba-a6e5-c5c5a08b5ef5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "68ae9b06-7a15-4ba6-a0b2-82fa9dbc0ae9", "d4e7db86-4850-4a42-b3be-f52573a0a165" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "890aff7c-e4c7-45ff-a428-dee61c503ffc", "d7a1da52-a2f4-43e6-b3a5-75304682522e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f6158d99-964b-45d1-90e2-3e2b9f5bff85", "1763bdde-b47d-4b7c-90c7-2dad0cb702df" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ca2af035-2fd4-4248-a40e-d375058d26bb", "4a4993e2-1900-4f04-858d-756b97f86ac5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3b775f6c-371b-400d-9c5c-cbb42290fc27", "4d70cf18-3685-4a56-8f24-0ea2fbb24c87" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "93058fcf-e1aa-4b67-8b76-0b61f12a9517", "ee98cd98-3658-4d31-b044-b31e1490d0d4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "99f729d6-7243-493e-b068-4ff4d574e3d3", "f369fa17-edf3-43c0-91fb-0968782db4f7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c7fc05f8-36fc-44e2-99b5-693495eb807e", "ed9aafe2-f37f-486d-8591-72f72e20b833" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2c53a2c3-923f-4b88-8ed1-c00967ff03f8", "0b2e6510-e0c0-48b5-b6d9-a4c2c8e4f2dc" });
        }
    }
}
