using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class TryToMakeEFCoreRetrieveRoleDataEagerly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
