using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddEmailConfirmedFlagValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4b74547-1385-47eb-80fa-1c29d573f571",
                column: "ConcurrencyStamp",
                value: "c98db791-bdc8-44bc-9da9-e3510fa880dd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f0b2115-c519-43a1-8aa9-8c6208aca71d", true, "AQAAAAEAACcQAAAAEPzZj0xpM6Gp2UxufrD6cmLXjUy6/Q5aSVpYH6OQjtjuW3SX9OYbODlpj8PoOjlKNw==", "da744beb-55a9-45ee-b2d7-5b4b7c245b4f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d2b29c91-540c-4c56-a62d-daf8e453331b", true, "AQAAAAEAACcQAAAAEGOq1B7N1LYTcQMqtgWqFE4kqcqbs0vscn6sjf60J/NNG2vbbcmxBfh5NjHeTVuxVg==", "d98e7a51-7824-4b1b-b41a-732fdcfc5164" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6a28daff-1212-4a3f-9373-9fff748f405d", true, "AQAAAAEAACcQAAAAEEJHbtZJTCkDjDMr2+TEr18nylQwhelCYW7ZoRL/i4Q4Y2jmMLZOjoNFosxMZP3vvQ==", "dabe2d6f-9e72-41cf-8c37-15407d82959e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5077d536-44a7-4930-9a56-3f95b70a0987", true, "AQAAAAEAACcQAAAAEAm8UIVaYdKbeanCpgYd7m+myosSCykLNVTbBhV8GzlvJCLRdYwWdqGsneIzLRzrFw==", "4a76a077-cc8e-48a6-a576-bb7cd4e11627" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7cff7787-6631-4de0-b4ce-0c479f7c4af9", true, "AQAAAAEAACcQAAAAEBZ2YS+g+VEV/GjUrwXCoA7Ad+ZHTwtyTAkpDjItqOtOOSWgpdY1YaiDIT1RXuwP8A==", "defae789-e827-4bc2-a2ea-2d9f9140892a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "832a6753-6959-42f9-9e55-b00edf892d28", true, "AQAAAAEAACcQAAAAEEY40GJR9Od/EByNVQDTyhcx+Xvq7+ZKRwnYw5w2s2km+ynzbaGq+sVxM871sX8stw==", "1ba04a06-0fed-45bc-9d2b-589d008d3a6e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bb700382-2840-4356-936b-4ed18e4522b1", true, "AQAAAAEAACcQAAAAECXfnC9Trf1fT0vURIn44q/J/X9ygD3rxqa1WhxonvwNEltpc0Lr62gk+dGSSf8HyQ==", "f9f7dacc-781d-4cc0-bca3-28b2aec453c7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54ed019f-114f-4c68-84c3-b1dbac648081", true, "AQAAAAEAACcQAAAAEHgj7afa8UHIHCQz7qms+slbI2AyT+B+l124IuM5XWwpiQ/wx5vG55pwzt8P2AHsjw==", "d1e6ccd2-f1f8-41ba-ae23-6506308b52d1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "72a69f6e-f6aa-4c8b-8cce-d960c318c41d", true, "AQAAAAEAACcQAAAAEFONJJLkcS7CdMUXzrDIymJvlbzAlPR5h2ghScbamKfeg2c/UKFZgz6x13VyD+zU2w==", "39028d36-548e-48da-ad92-dbe75e2317c8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "acc49a9e-785e-45b2-ad11-9885f881050f", true, "AQAAAAEAACcQAAAAEJ4IdTQeqF3zVMkeNrL5LC1M+ioHP5BWToPHxdHDlGtupDbm3/dYgLAHPAKQapWyxQ==", "fa78facf-58bf-4e57-975a-56c007c84d29" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54fa3f74-3e4f-47bb-8bed-8a0a194bf5ca", false, "AQAAAAEAACcQAAAAEM7CITctZipx+pc4xFW6M7p6MoVPGR1k9/PglA73ARpaq9SHS8NQ5O383mha4/nfjA==", "1640cb9d-483a-4f84-a87f-df65d50ce0cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4adc59e7-3334-49bf-b860-a79e543cc925", false, "AQAAAAEAACcQAAAAEEiwhvV1VbOMvBQXzc6FXU5pl5uPkgLRta1hqSNdiA8y22ZjcuTTlcK4rlWfRTo+rw==", "7ff9d0f2-9310-46d8-9069-f80c91f1ddfd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7e71688-39f1-4809-831e-20a90de753b7", false, "AQAAAAEAACcQAAAAEMFwQuN6S8PkFjhJgHB+3K/KwTZbr2M3Lp5YkpCLT3fmFKu8DwStbPUwwNeUitjZEA==", "5d48614e-dab1-42a0-a3ce-bd60f5ac4465" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e43b8727-ab76-4d48-b4f8-7591572cb6e1", false, "AQAAAAEAACcQAAAAEMORTmF9/UdAi6tLQVehwNaSc43czPiqTCZAy6mDDBXJEs7d7sdrF4aJsMSLFBx/qw==", "91ca184b-146f-49bc-b3af-61dbb05fbb62" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "75993074-eae1-4029-8592-04728f55dc35", false, "AQAAAAEAACcQAAAAEP1OS1y9RdCnE54ZYST95iHIbrYoQIme99BGcTzSzASC0FO/oDRtfKMSxxGlUgleTA==", "d18926cf-2bcc-430a-b484-84c642318e8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad195763-0f5b-4027-89fc-5892b66496c6", false, "AQAAAAEAACcQAAAAEIYtgwqMcAXgWgGe0TOCR1zJzAKi+BgnXqAzdy8gziI4s2lq15s04EEQaQln9xuk3g==", "49dcda3c-c006-44ed-9aba-f108c609a101" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6f7abcbc-d257-485f-a84d-fecc765bd6fe", false, "AQAAAAEAACcQAAAAEAHDT3QiDugCwv/oZjwKvrO78ACr7t0QjQfmm27WDwrVCCP1ypEFQ8PFMR6JgEZVrA==", "9d9e0da2-b415-40d0-8974-9a56803c6805" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "38e0cf45-530a-4cc3-9fd1-2b044b5392f7", false, "AQAAAAEAACcQAAAAEI6tno2NBkN/nqEs+p+Tm116kHElEpaMAUZcSLgiJNcHJmVasQBY9SXdvSqrQ7DmyQ==", "076a7b4a-f89e-4626-ad5d-c740483a4d9e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a2fde02-f5e5-41ea-9129-58048b4b532d", false, "AQAAAAEAACcQAAAAEDaNjE+lbHtAyMX6lGHCfMvQDOF9yFsbFWmz3ydS5SReqG76+Kk3WcZceWN+W7VQQA==", "e51aaaaf-22ee-4002-a24e-88ab7bbab767" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69c0eb2f-a198-4469-9a21-ea030a253d73", false, "AQAAAAEAACcQAAAAELxY1N41naD9egZQE4jdbeJJ7rgVFfUG1iHCZzh63aVWHSvT/q9uXrxKh2nqvyeeYw==", "bfa98824-1ea5-44c7-ba1c-a20bb7b4f56e" });
        }
    }
}
