using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class IncludeAutoIncludeForUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4b74547-1385-47eb-80fa-1c29d573f571",
                column: "ConcurrencyStamp",
                value: "e601cff8-db8a-498a-9680-370c6a478d63");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7ab66453-f074-4274-8b9b-52c3af47be19", "AQAAAAEAACcQAAAAECxcL8OSDW6waulSq51Nllio6JL2zc0zHqTdMIrwEzaGb/SnalwoDKJTplzbtkcynw==", "9f2fcf68-854c-4d2a-b8e6-e56e46561a92" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77fe2eb0-601a-4d95-bfe3-6bb89982109d", "AQAAAAEAACcQAAAAEDP38H+mZhJM2P4LnKbK1qHvFIUPbUyJgy1A5Nt+aBAk/NpEDHI339qN+6yB+tHTbg==", "c3a478d8-bf96-4d1c-b7bf-d7d85707ad4a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80809028-81da-4302-b7be-186a831a45b6", "AQAAAAEAACcQAAAAEMh+s4PfOKXM+XLrnqIRByfd/BicNww3dhcTjOCAXkZbt7z3PrCYj21Z6iTOp7iGfA==", "b8a780d4-695e-4ee2-8f3d-d313b3842022" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c7b45e8-2c25-460c-bf1f-9c708f660b3c", "AQAAAAEAACcQAAAAEMalc1BPEl7U+OQmr38I3sbuSzVdlmkzgZ4Sp+hUYTAVugHCUqeTU6tvInJlF9m8UA==", "ae73f7bc-b976-44fa-9ad5-f3a4c1e1f1ab" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3aadd4c2-6da1-4d8c-bbba-3a67b3f805ba", "AQAAAAEAACcQAAAAEMo+H7/ustSejVxNajZFA5THpD30xD1e216suDn8OI3L2hv2dyn0XnpPcPMLPS1/fQ==", "230bfec7-6327-4994-94a8-edaf2e944e2e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "333e0d23-e17a-41dc-b004-3e49bbf63f67", "AQAAAAEAACcQAAAAEAy+KShr5YP90ygQRSMSXpxFzhjk73errnc+7R7LEvf/+94r6cJ2cDauYPsmfEJniw==", "851f3036-4b10-4eed-98d1-e81d011a30a6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "feae34d7-b78b-427e-bb75-07b3511fb6ee", "AQAAAAEAACcQAAAAEFyGllf2n+behmvcEksDtsCP5FnkCOd9MrXVecJYD1F1uYMOwItpb/FLt65nGwGqRA==", "e7620824-3499-4691-bc46-7b1424ade700" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cddb2a63-a407-4ff1-8054-df4d168cfa3f", "AQAAAAEAACcQAAAAEABsPuidDmM4SqL0yJnHpsJ3dSInr42tO5GALMEwD+GQPIRrDH0jHRi4ixpGfGgHJA==", "813d7a62-2031-4e68-b5f1-c27fe3305914" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "97be8af3-a984-4eb8-b8bf-1ef340804922", "AQAAAAEAACcQAAAAEBWf5RhUbdx22OKzqd7Je7gSH8h61uYj7ZoNd/oY9Xb3vNYPxkSywmE7wCSjHBuVbg==", "6f3de0d9-2678-48d4-b481-3827e5b39042" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e3762793-6a33-43d8-824b-f362549cc2c9", "AQAAAAEAACcQAAAAEGkLQziWLHlqdVUgR3YvMYFUbxH5miTxhx6RXMHglHeZmvfDDgQcCrNLmwEqZScftg==", "fb7b3c7a-d99c-43aa-873b-7ca4deb1460e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4b74547-1385-47eb-80fa-1c29d573f571",
                column: "ConcurrencyStamp",
                value: "613fb5af-c6c3-40b4-8b15-5e5fcfa2fbea");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "108677ac-4caa-48d0-a7e0-4c95cd7362e7", "AQAAAAEAACcQAAAAEMy/ofYjXUwlpga5aa8F4On6zfK1CfTWWyiak+Sv9YL5fB4gLfE3fevWnwaKtc/kww==", "40aa1a1b-28a8-4f18-bae8-8758f20b6d98" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "66eb2259-6ce5-44b1-ab90-e3fa9b62da51", "AQAAAAEAACcQAAAAEEe0zdG96S2mpUsYU62KMjD4B9OZnCNR/luU0XSWethQdoT+TBzuaqJCfErCQvMQqA==", "2aac074e-9207-4ced-aa45-2c08217d173e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b1f7f0f-a1dd-42a6-a2bd-20fe60f5984e", "AQAAAAEAACcQAAAAEABFPWF0ab/pAVj+07LbKjV68RdMygdetVEENUWFiubYX+rh7DU1CgZmv3rUBDyCEQ==", "28bb095e-cc43-4bce-b4c6-bd5e71849d67" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ec6ed0e-16e1-4ee1-906a-b1ece2ed955c", "AQAAAAEAACcQAAAAED9lbz/GxdvyjO6OEo4QKYjlWfOe6ko/pYwoLuO65dtlmxd3WOcgQkOAMG6dScRcLA==", "7c07658c-f065-41ac-8e3e-eb7545c77bc1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fe7d2e0c-2038-489f-af07-e2071d62e583", "AQAAAAEAACcQAAAAEK08iPMQVqFctk0jEC62rvHarHjNtlU6WbBY0YxXRqrqEanTml2OZfw4Wma5v1qDVA==", "6a3744e9-3e10-4da3-a991-cdba25bdfd8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f56ae1a2-690b-468a-af0d-5362a75bbb37", "AQAAAAEAACcQAAAAEOdvWUq3Px0vC5ISo1k3bZul1eI9pBhGyj7Lzhvw+e33VBUktMHVXrnoLEyCNipDOw==", "42d8134b-a0a7-4328-a15b-5f4dd630deb8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c563d857-e685-43d4-9077-7cf070a4ac11", "AQAAAAEAACcQAAAAEHplR8jc5/txCf58Zmy+CPTzPokManSZxc5b2x8u1EHedQivBXnwHTSCV6q/1A4cGA==", "5f166a10-2679-4138-ad22-1252def724a6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f046239-267b-45bc-ad64-b0ea23372767", "AQAAAAEAACcQAAAAEPJsyjlJ1yQZgflnNg3NeskkHCtUNaZTzexwez061hHknAY3yhvLJeKf0ORy5/GmJw==", "df0764a0-159a-42fe-b0b2-d3af6401f91b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b311920e-8df1-4a8d-9d8d-1fa294c30230", "AQAAAAEAACcQAAAAEIVF10lavmqizs6Cxsj2KNER7Go3hRykisLtOBgx6tMOw5eWIcHgvRGaI2VU0gLhvA==", "f6208ddb-f3f8-461b-ae35-7cb74e241a48" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0693db44-745f-449a-9f59-49e101672a22", "AQAAAAEAACcQAAAAEICjUTJT+lKVBWY8PXjxndbFl0eahf0y2cZ0SzG46tPKObt5TZyEwuXQmmUhyBfZ+A==", "caf837be-8111-4b47-b8e3-5e3579ca63eb" });
        }
    }
}
