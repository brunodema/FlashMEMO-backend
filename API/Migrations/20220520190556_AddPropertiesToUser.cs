using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddPropertiesToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "bcde98ba-b5c3-43c0-a425-fabfe13ba306", "", "AQAAAAEAACcQAAAAEEC6PuuZsy28cTUG9FszHFkYfMJ5qUZYIz/MkGMviAvTvVLOwB4v1HIGuCb/9Zivbw==", "90930c3e-3c17-438b-9b41-1b3567eaf481", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "86522fb9-4f8d-403c-aefc-51c4d27044ad", "", "AQAAAAEAACcQAAAAEOAP0NhCBSiynJ08Arg3mWR2hoUIhUtf5n0IWydigtaeMX7MqGOPoghrvixsN8MHXg==", "74b171a5-8f58-45fd-a41d-8f9e1a533a4c", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "d54c7462-3430-459c-ae31-a5b8992ee63c", "", "AQAAAAEAACcQAAAAEFNPSCuhijfcEF4YDgzdHeU4IeNydvLNA9LCNoPPZ/+NzqovIsjkEM3nHokQ35yqVw==", "22b74d03-71b8-4da7-960b-b7fea590d87a", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "0d053b03-6fc8-4cef-96eb-5b307ee71c6f", "", "AQAAAAEAACcQAAAAEG8qIqOrgOeyM45wChmQfIzYRpyVHskMQfnxHoDur3hmh6UxsEoUzqzzTgFULfB3eA==", "9a8bcab6-3bd1-4c8e-aca5-f953428e38f7", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "1622437e-f4de-44b0-b62e-1039dac8bc33", "", "AQAAAAEAACcQAAAAEN70gm1G7PHYRyuOGRaG2f8RkuFr+SLnlo9IgCAjml5RzBYi86LHJq2SBr9Mz4TKhw==", "89d8f95a-967d-4eed-87d8-735aa2fb2b01", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "45cca3a7-3f48-461f-b199-8d583fabdc8e", "", "AQAAAAEAACcQAAAAEBc9i30pf4EPF5x7oF8rGSU5CNMvb3AhJaYkZF0yvsqAzME+caGGHZnU3SoKN7ZODg==", "b3f9d20c-7861-40e9-9267-e273d2e80e9e", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "b7532fe3-3c0c-4ed5-8e45-ef3981d71d06", "", "AQAAAAEAACcQAAAAEDFDF3V1BifArpNQaM/Gc3Qbb16KsOA7Vdpggkkjuo4Rlkqe0kZtppS+EqB4cXHa+g==", "04303495-2733-445c-a996-06b664d6aea0", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "8d894b57-55f3-4501-b83d-9eda6d47b4da", "", "AQAAAAEAACcQAAAAEJKex/fPnwcoxp3+4Zd6WX0dSFSVzR/xaXxJPPIAFNATH/kC5LLQNnRO1KLxesCJNw==", "0e1f09ad-1357-49a2-9737-b36709371eeb", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "fc1e4aef-3bf3-4e5a-be5a-c91757dcc071", "", "AQAAAAEAACcQAAAAEN3g1OmXYk7GJjTtbr+CVpq+sNtOLyyiZPK6aLeO5r8njG1hHwAWyFfhE5qX6cdVpQ==", "49d820e4-9ba0-4809-946f-04d5cefb07e5", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "bc0da8ce-8e01-4a6f-9e9f-e4949d6bd051", "", "AQAAAAEAACcQAAAAEEfYDRH0QmNeV0oDxBvx3yITKiY27k5zeSfiyEqc0Rbum88jRBwc7U2ar0qX45V/4g==", "96417029-4964-43b0-a31f-3c1d299a7b5c", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "52980151-9872-4f76-822c-cd30328dae94", "AQAAAAEAACcQAAAAEMqyJTAFe2woFsm3GUs6ubWcnsDRX+eDUgWdzX/AXMvjkxEbLIKqmH0Sk7U6ylOX4A==", "0a4722b9-2058-4a9b-82f5-05d86fcc46c0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d7c8bf4-f155-4233-b4ba-226ddcb7886b", "AQAAAAEAACcQAAAAEIoPo7vGTgF5/yOysBBf2TucLT5G4vlqu/MZ1I4vLd2AtGMTCSBu/ivO7P+4iqfmsA==", "ed3a2379-8509-4183-9dde-ccd617974a7e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1ea0e071-bd80-432a-945d-bb208eb43b72", "AQAAAAEAACcQAAAAEOUIh120iEWQKhOmRtHWP6IMs+gMeaS2NHPbhctklY2BqCkeLlnCd1WU1MzwDw8lyA==", "554298a4-7155-4f8b-a258-f3e04ae50d62" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddf78a67-0254-4bee-902a-bb79975e03f6", "AQAAAAEAACcQAAAAEGLZmoCWew0oQ42t6WW+Z4wUeh8tRc1twv6mHoUK70ccNrjhndpzZGFE17DzftsuOA==", "3e0a7f6f-ce2c-4575-aad6-c6e14c7bf5a7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c390d1ac-6e40-40d7-b4a7-f50f9a3a62ea", "AQAAAAEAACcQAAAAEM93xSz39aPSe1TYDIWSNnm2Cz0ZfDMynGMkpLyb82Z/v/zBiQyqZvJB5H9Qp4pIgA==", "2344d216-e7c3-43d7-97ff-8fdab3f603fa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98625a1e-5e02-466b-b6d3-65f454fb2ea8", "AQAAAAEAACcQAAAAEAQZE5nD6sVJsvST4zlTzJm+sUTySIz/b84nnIA4l1H7D4UffFTB0GLdatJGsRepwg==", "5c0557bb-2145-4f95-82c6-5d49c61b8936" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "384ad32a-172c-472a-8afd-fbb1632d4743", "AQAAAAEAACcQAAAAELZRlaKKvHmBnkhihG3GrsXOJKr9CBO1u97LP2Ty4JrZ5A8TT0UYXCFxQZmWjOwjfw==", "71cdb436-9bce-4dda-acdf-93e401f8408e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5dc131e1-ec58-4366-88ef-16da2ca0229d", "AQAAAAEAACcQAAAAEDXYEabnoZ6BS8bfx6mqc74a+NEgVP28ZV4jJ5LL3fzJKUqAP7sVRy/Bl8Uk/NgzZA==", "730b2c73-70be-4687-8573-ba4c7efba268" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7b541f1b-a21d-409b-9651-f0c3efd771f6", "AQAAAAEAACcQAAAAEBnD0DHWaqMxXVdwd+Y4kGOLoVKTehpmkLSN9wTtn1yaSaBqyDCfJKSlhDlnJEWbgg==", "d4f69e45-0300-4870-9dc6-4a352ea0994a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "665383a9-22a9-4e6f-a7ba-a8455d99eb24", "AQAAAAEAACcQAAAAEDdEbgnzU0HLRZDhakX+KZ8nANKnKcA7SF+xFzTuwhPwKKFJvD2XfXMCmDid18REbg==", "a59f816f-9d08-4bed-a153-f3fdf82798c3" });
        }
    }
}
