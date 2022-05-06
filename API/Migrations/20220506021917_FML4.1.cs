using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FML41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "32c319a2-8795-468c-859d-b235234cccb0", "60ea3a05-23b5-4c58-9490-6bbde0ecf0cf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "276bb0b2-ae59-4f17-9a38-8f9aa33eab0f", "a23b491b-3288-40a2-a0e9-ce2cfb07b80f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "463f3e2d-3628-4bcd-a7c6-438e94a70fa4", "8b8c4632-44db-4210-8e81-96a586faee3e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7c52c714-e766-46ea-8b10-e0b8b1170ae3", "8c788906-e695-4678-9d50-b93ce021456d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8ec137dd-0e84-4c74-94b3-7947a3564356", "4109f891-27bf-409c-a595-a2f99b5947ae" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6a01669b-008a-492a-9c7b-d4a03380dc91", "f3140d7b-3b40-4d9e-a7a0-e558b69d7a77" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "477bba8d-c286-499f-a1fa-9bc156dcb955", "88b95cb1-ca98-4f8f-80b5-bd702fbcf547" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "82d74992-aa1c-43aa-9df7-451acd679c6c", "822b0f77-3735-414c-b416-9d04dcdaf218" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1e52c18c-4e53-49c0-a34f-796cfcc704d4", "c310747c-3f3a-4691-ab4d-81f2276e8491" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2153f81e-f276-4c1e-8701-2054b7a845be", "f0ab0ab5-79e5-463e-971b-f404cff14674" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "08d88900-4473-4c5c-9894-0b6f86dbfa4f", "d99cbbab-13d8-4d38-8f43-321c74b01d09" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf436c3f-bc3d-4e0d-8fd8-8bd26770db00", "4adb1589-50ae-4c64-a283-113f855aa240" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e9faa793-33cb-48d3-823f-9b88c141195e", "7081c7e2-064e-4426-8532-5ba3305e4f2d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "89e8a2da-48ba-40af-ac46-6168a1dbe6bb", "13aafd2d-6c3f-436b-bddc-81b191e3e40f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "80315212-4467-4452-b730-ad3041136bf0", "84a19aeb-dd56-4f38-8925-e06cca44b58e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7e595cb7-5679-47da-a51c-aee2f9d551b0", "6d32d611-58db-45cc-9add-3bdc997ce276" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "21f72480-dc88-419d-b2b8-d0053af70b54", "e98a7c4b-9cc6-4d62-bad8-54fa5a6ae695" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "66a2cef0-1dbf-4a6e-9cab-0773a7bafc93", "ead88c7d-bb89-4b76-9bfa-af3769f29c8a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "97d624c3-010e-488c-af65-e2c95c3ee6a4", "7111b55e-d174-4b3e-b3be-a23f89a2f658" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "de103469-9782-489b-92ed-d4f947db0a40", "3abbde8a-57d8-4a8d-b08f-c0b5bf494227" });
        }
    }
}
