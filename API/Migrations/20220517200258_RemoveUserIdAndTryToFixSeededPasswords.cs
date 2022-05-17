using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class RemoveUserIdAndTryToFixSeededPasswords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c9c70d1-144d-412e-8b07-d9317c485557", "AQAAAAEAACcQAAAAEHc7qXLdoZEmaiRwU/wio7QRSp80pEs6leetJMDhaSVsUI2wI1jHRZ9+c3rhK7gpkg==", "547c0169-2500-44a9-89f3-294caeacdd24" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "175c6261-8214-4518-b2f0-1427077572a7", "AQAAAAEAACcQAAAAEOQ0e+O3fGX0R/N1FhYyeRT1ZRgb1iBI5higdS8M9WbOCQTNFYusLQamiIJ+8PWSSA==", "e5a10a1f-d9a0-4bf5-ab55-abfae50878fa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "323b00ed-c3a0-4e92-a50c-356da02d16bc", "AQAAAAEAACcQAAAAELX6LtUtf8TMNUjmUy3ldFlc9xqi3+9vs0a9n///430MljqWjY5dJ8YE3KGgr1aqZg==", "afb49b1b-4de4-4551-ac51-bfd1c472ec78" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7794acca-e7e2-4485-9a28-bb450e15ae27", "AQAAAAEAACcQAAAAEMsVSxcQdUGOQkgTG0lwnEF7klI7VGe1BLsrTfw31mKL/GoJYwfp1rziDMNanNiN4A==", "8391c08e-894e-462c-a3bf-b33ddad00f7f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6404548a-d8f4-421b-bc08-9a84be2c49b9", "AQAAAAEAACcQAAAAEIDEyglZ6KGTHuW+ltc/p/gYUAmMnizTINuI/9RoHqPg0mVTdrJshy1QkVh2CqMcAA==", "a0cd10c6-fb09-43e5-8ef3-431b563b1c59" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d21f9113-5187-462f-9e05-913befd03e38", "AQAAAAEAACcQAAAAEGKafCd6Lg1bQQl5CM0amgG2aMYxCzQLVPW2Ept/hPQXtNnKBYqOvNdGlqRtWlb6Zw==", "05476e7c-2ed1-4eb9-b211-76c370285a24" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db613d01-f16b-4546-be07-cfc1a6b81731", "AQAAAAEAACcQAAAAELs0WRX7x626bpbija+N8RhB0X9fNBihGYyPj3ZuRF8JLq1ckKtHZbG1OLib92+JnA==", "a872a8f4-dcc1-4d21-8003-2ba22c5d1150" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9248788c-e8bf-45c1-a142-30ad5b19ae38", "AQAAAAEAACcQAAAAEM+q41ONS+lTdgVh79awIcd/uLiiaBDSdrB/ci5aBzjciRe9JPr5Ry8zPzx6uY+3VQ==", "287b9764-7679-47b4-8a80-33ea0857e8dc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da737e5a-105c-4810-a19d-e6229a761155", "AQAAAAEAACcQAAAAEOVWCAQ0xbypSxQQG52OaEfffUbkkWw9zUybbD3A8x3GKHbkMAvx9h/JujD+s0xIag==", "c5dada8c-e4af-40c3-91b5-b0cd3ba7447f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77b93916-17de-40c8-abff-3d5ac7167847", "AQAAAAEAACcQAAAAEOHLtLAL6bI3BQV+XWiQFhi7TNNJVbpdpIXSogoJmrQ2adhpW4brV8/jJfIe7w+PUg==", "47cf986f-f404-4998-bc94-6650c2f79e60" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "8162fd12-ba87-4581-8308-ce0459176494", null, "4c7a204c-5b51-4205-a588-25f0de5ca9ae", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "6d08ce5c-25a1-4eff-baae-6833436d2f6c", null, "8cf87d73-a292-49fa-9824-b26bc1067dc9", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "903b0df8-dd06-403c-b71e-f1af92893a9d", null, "02584050-d4c1-4d16-82c1-f021f9f06c7f", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "b0541d1d-ff34-4d82-ba09-6e3fcd95a223", null, "22b145f4-8cd2-4cbb-9863-b54c2024d80e", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "27be1582-f9da-42cf-927d-303cf79939db", null, "e81eee27-6b4f-4684-8061-482d0f811233", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "298a1b8e-7c83-4e0f-b9e1-a97cd9a105ec", null, "ed38b57c-148a-4742-aab5-1e94b3f7a53e", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "5cc4040f-3382-40af-8a1c-1eb7e798bfce", null, "66957ad4-e529-41ba-8300-4ec9e42912e7", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "006719c4-f10f-4c57-8aff-23c43f9c83b3", null, "b39d694e-8ae3-4990-8fa2-a172dd3555ff", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "6a1f1417-9a29-4976-aede-8b21620becf3", null, "b8836ff2-5eac-4357-ab37-00654e5c463d", "00000000-0000-0000-0000-000000000000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "3e89028a-2ec3-45c4-87cf-69167f35de8b", null, "2c8e6e45-109d-42c7-8b62-c2573c07accf", "00000000-0000-0000-0000-000000000000" });
        }
    }
}
