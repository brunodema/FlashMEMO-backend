using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FML3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b73e7249-b177-4003-a2fa-034cfc564cbf", "d387ed35-9fe2-40e0-8902-b44e84c83000" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d8413489-6d36-48b8-9b34-4ec83f633756", "344bcc29-b845-4c01-a483-cd4215c7e1de" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "589110bc-b923-4739-97f1-3e2ea88e8a50", "abd5c2ed-47f7-4e50-a8a9-f66c1fe86882" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7c86e256-3781-4cac-8acd-75a43175ffae", "49b51aee-c435-43a1-ba88-b27c97452f27" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c9982c87-f807-4895-96e9-8e23efa4e3cb", "2922d95f-d9ac-4255-9f94-9adc727f16c4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cfa7ba25-55e1-498b-831a-1331dc8ae1fd", "79f3a13e-3161-425f-b021-fd91b908731f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9ed25a27-fbfe-4669-baf9-a34f3b9e8168", "a759de2d-35fa-43bc-ab24-8310ccd238c9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "caef0599-37b3-411d-a446-e5c3729145a7", "9bbd9b34-1f35-4419-8b7d-162c705e78e5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4866f1cd-5782-4daf-b4bb-2b424154f54a", "27c75d47-a6f7-405c-8271-2403a1abfa04" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e6d6ec0f-7d7e-4287-ac1f-ff63364d6333", "27a4ff73-9c21-4e1d-ad34-090f0547f58d" });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "DeckID", "CreationDate", "Description", "LanguageISOCode", "LastUpdated", "Name", "OwnerId" },
                values: new object[,]
                {
                    { new Guid("092bf87d-9de3-1bea-4fc5-f76294251979"), new DateTime(2021, 3, 27, 3, 17, 12, 0, DateTimeKind.Utc), "Donec vitae erat vel pede blandit congue. In scelerisque scelerisque dui. Suspendisse ac metus vitae velit egestas lacinia. Sed congue, elit sed consequat auctor, nunc nulla vulputate dui, nec tempus mauris erat eget ipsum. Suspendisse sagittis. Nullam vitae diam. Proin dolor. Nulla semper tellus id", "pt", new DateTime(2021, 10, 14, 3, 3, 6, 0, DateTimeKind.Utc), "Grammar Adjective Lexical Dictionary", "6BBD068B-B258-EB0A-2132-BEB9BAEA886E" },
                    { new Guid("0e275d7c-c765-a566-c091-3ac90c4f2a72"), new DateTime(2020, 10, 21, 6, 49, 21, 0, DateTimeKind.Utc), "Mauris vel turpis. Aliquam adipiscing lobortis risus. In mi pede, nonummy ut, molestie in, tempus eu, ligula. Aenean euismod mauris eu elit. Nulla facilisi. Sed neque. Sed eget lacus. Mauris non dui nec urna suscipit nonummy. Fusce fermentum fermentum arcu. Vestibulum ante ipsum primis in", "ch", new DateTime(2021, 11, 5, 7, 13, 44, 0, DateTimeKind.Utc), "French Italian Portuguese", "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2" },
                    { new Guid("1280cd10-110e-d2c5-bbba-f6eea2c1b3e4"), new DateTime(2021, 1, 24, 6, 42, 59, 0, DateTimeKind.Utc), "dictum magna. Ut tincidunt orci quis lectus. Nullam suscipit, est ac facilisis facilisis, magna tellus faucibus", "it", new DateTime(2021, 7, 8, 3, 37, 35, 0, DateTimeKind.Utc), "Adjective Lexical", "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7" },
                    { new Guid("18d5f976-3c81-1711-ccd3-1ba2e7caa24c"), new DateTime(2020, 8, 6, 9, 31, 21, 0, DateTimeKind.Utc), "turpis. Nulla aliquet. Proin velit. Sed malesuada augue ut lacus. Nulla tincidunt, neque vitae semper egestas, urna justo faucibus lectus, a sollicitudin orci sem eget massa.", "it", new DateTime(2022, 1, 30, 5, 47, 40, 0, DateTimeKind.Utc), "Study Quiet", "BD696EC2-1BE1-FFB1-3FC1-883C2D247875" },
                    { new Guid("1a44b87e-15c9-9ab6-2ba3-74623b8a1513"), new DateTime(2020, 9, 19, 5, 33, 56, 0, DateTimeKind.Utc), "magnis dis parturient montes, nascetur ridiculus mus. Donec", "hi", new DateTime(2021, 8, 31, 5, 17, 51, 0, DateTimeKind.Utc), "Portuguese", "5881A9B3-912A-8F1A-C5FA-0855A0563E23" },
                    { new Guid("1f511e25-e470-6b8c-a283-c5f44fc61936"), new DateTime(2021, 4, 17, 22, 55, 25, 0, DateTimeKind.Utc), "ac mattis velit justo nec ante. Maecenas mi felis,", "ch", new DateTime(2022, 1, 21, 14, 57, 45, 0, DateTimeKind.Utc), "Text Grammar", "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8" },
                    { new Guid("29717e54-7223-f856-cebe-5e5ee2f187b1"), new DateTime(2021, 3, 10, 20, 34, 15, 0, DateTimeKind.Utc), "ad litora torquent per conubia nostra, per inceptos hymenaeos. Mauris ut quam vel sapien imperdiet ornare. In faucibus. Morbi vehicula. Pellentesque tincidunt tempus risus. Donec egestas. Duis ac arcu. Nunc mauris. Morbi non sapien molestie orci tincidunt adipiscing. Mauris molestie", "it", new DateTime(2022, 2, 22, 13, 43, 12, 0, DateTimeKind.Utc), "Italian Portuguese", "5881A9B3-912A-8F1A-C5FA-0855A0563E23" },
                    { new Guid("2a4424cc-2c1d-1309-a647-be27769a91c9"), new DateTime(2020, 6, 17, 13, 1, 52, 0, DateTimeKind.Utc), "Vivamus euismod urna. Nullam lobortis quam a felis ullamcorper viverra. Maecenas iaculis aliquet diam. Sed diam lorem, auctor quis, tristique ac, eleifend vitae, erat. Vivamus nisi. Mauris nulla. Integer urna. Vivamus molestie dapibus ligula. Aliquam erat volutpat. Nulla dignissim. Maecenas ornare egestas ligula. Nullam feugiat", "ch", new DateTime(2022, 2, 22, 22, 42, 40, 0, DateTimeKind.Utc), "Deck English French", "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4" },
                    { new Guid("2a763e3c-0411-1fd6-2719-681eca29259e"), new DateTime(2021, 1, 14, 11, 44, 24, 0, DateTimeKind.Utc), "a ultricies adipiscing, enim mi tempor lorem, eget mollis lectus pede et risus. Quisque libero lacus, varius et, euismod et, commodo at, libero. Morbi", "hi", new DateTime(2021, 5, 4, 0, 37, 29, 0, DateTimeKind.Utc), "Italian Portuguese Study Quiet", "6BBD068B-B258-EB0A-2132-BEB9BAEA886E" },
                    { new Guid("336bbabc-4a14-b13c-2958-3c98335a354f"), new DateTime(2021, 1, 28, 16, 5, 27, 0, DateTimeKind.Utc), "bibendum fermentum metus. Aenean sed pede nec ante blandit viverra. Donec tempus, lorem fringilla ornare placerat, orci lacus vestibulum lorem, sit amet ultricies sem magna nec quam. Curabitur vel lectus. Cum sociis natoque", "en", new DateTime(2021, 8, 12, 1, 37, 0, 0, DateTimeKind.Utc), "Study Quiet User Text", "BD696EC2-1BE1-FFB1-3FC1-883C2D247875" },
                    { new Guid("454177ec-70a4-e637-5d69-46202503c7f5"), new DateTime(2021, 1, 10, 13, 0, 44, 0, DateTimeKind.Utc), "Proin vel arcu eu odio tristique pharetra. Quisque ac libero nec ligula consectetuer", "hi", new DateTime(2021, 5, 9, 15, 31, 18, 0, DateTimeKind.Utc), "Text Grammar", "6BBD068B-B258-EB0A-2132-BEB9BAEA886E" },
                    { new Guid("4942c531-e221-1d68-18d4-dc53b48eb51c"), new DateTime(2021, 1, 30, 18, 45, 46, 0, DateTimeKind.Utc), "dictum eleifend, nunc risus varius orci, in consequat enim diam vel arcu. Curabitur ut odio vel est tempor bibendum. Donec felis orci, adipiscing non, luctus sit amet, faucibus ut, nulla. Cras eu tellus eu augue", "en", new DateTime(2021, 7, 11, 5, 50, 48, 0, DateTimeKind.Utc), "Italian Portuguese", "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7" },
                    { new Guid("569698b2-60f5-4abe-4a8d-ec21259cb152"), new DateTime(2020, 5, 7, 1, 8, 23, 0, DateTimeKind.Utc), "hendrerit neque. In ornare sagittis felis. Donec tempor, est ac mattis semper, dui lectus rutrum urna, nec luctus felis purus ac tellus. Suspendisse sed dolor. Fusce mi lorem, vehicula et, rutrum", "en", new DateTime(2021, 7, 2, 12, 43, 20, 0, DateTimeKind.Utc), "Portuguese Study Quiet User", "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7" },
                    { new Guid("5db32645-4444-63aa-c1dc-c059789b215e"), new DateTime(2020, 6, 11, 11, 45, 32, 0, DateTimeKind.Utc), "odio semper cursus. Integer mollis. Integer tincidunt aliquam arcu. Aliquam ultrices iaculis", "pt", new DateTime(2022, 3, 6, 4, 17, 3, 0, DateTimeKind.Utc), "Test", "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7" },
                    { new Guid("5df3869c-a2e7-88dc-4e4d-9c3fb0049ff6"), new DateTime(2020, 12, 25, 10, 32, 21, 0, DateTimeKind.Utc), "Donec est. Nunc ullamcorper, velit in aliquet lobortis, nisi nibh lacinia orci, consectetuer", "it", new DateTime(2021, 12, 23, 20, 30, 46, 0, DateTimeKind.Utc), "Adjective Lexical Dictionary Audio", "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167" },
                    { new Guid("81775da8-5104-21db-2b55-e1a1d8b4764a"), new DateTime(2020, 10, 31, 16, 40, 7, 0, DateTimeKind.Utc), "pharetra ut, pharetra sed, hendrerit a, arcu. Sed et libero. Proin mi. Aliquam gravida mauris ut mi. Duis risus odio, auctor vitae, aliquet nec, imperdiet nec, leo. Morbi neque tellus, imperdiet non, vestibulum nec, euismod in, dolor. Fusce feugiat.", "hi", new DateTime(2021, 10, 19, 6, 16, 2, 0, DateTimeKind.Utc), "Italian Portuguese Study", "0004E137-13A5-5129-AA50-5A856A2A86F7" },
                    { new Guid("85b91a36-6d3d-8578-1b4e-98389a57e1ac"), new DateTime(2020, 10, 15, 18, 6, 39, 0, DateTimeKind.Utc), "amet luctus vulputate, nisi sem semper erat, in consectetuer ipsum nunc id enim. Curabitur massa. Vestibulum accumsan neque et nunc. Quisque", "fr", new DateTime(2021, 10, 10, 12, 19, 18, 0, DateTimeKind.Utc), "Study Quiet User Text Grammar", "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4" },
                    { new Guid("913df528-fb15-529e-5017-dae1c74758d9"), new DateTime(2020, 6, 23, 23, 28, 55, 0, DateTimeKind.Utc), "semper pretium neque. Morbi quis urna. Nunc quis arcu vel quam dignissim pharetra.", "ch", new DateTime(2022, 3, 12, 7, 6, 58, 0, DateTimeKind.Utc), "User", "BD696EC2-1BE1-FFB1-3FC1-883C2D247875" },
                    { new Guid("9963db07-6539-8a5e-aa87-b14c6dbd7631"), new DateTime(2021, 3, 28, 14, 35, 19, 0, DateTimeKind.Utc), "Mauris eu turpis. Nulla aliquet. Proin velit. Sed malesuada augue ut lacus. Nulla tincidunt, neque vitae semper egestas, urna justo faucibus lectus, a sollicitudin orci sem eget massa. Suspendisse eleifend. Cras sed leo. Cras vehicula aliquet libero. Integer in magna. Phasellus dolor elit, pellentesque a, facilisis non, bibendum sed,", "en", new DateTime(2021, 10, 31, 11, 53, 14, 0, DateTimeKind.Utc), "Deck English", "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167" },
                    { new Guid("ad56c6b4-4791-7982-4ac7-cca45243607e"), new DateTime(2021, 1, 18, 21, 59, 23, 0, DateTimeKind.Utc), "massa. Suspendisse eleifend. Cras sed leo. Cras vehicula aliquet libero. Integer in magna. Phasellus dolor elit, pellentesque a, facilisis non, bibendum sed, est. Nunc laoreet lectus quis massa. Mauris vestibulum, neque sed dictum eleifend, nunc risus varius orci, in", "it", new DateTime(2021, 11, 1, 14, 14, 18, 0, DateTimeKind.Utc), "Adjective Lexical Dictionary Audio", "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4" },
                    { new Guid("bcac69c2-5a7d-74c9-e58a-792964b6ece9"), new DateTime(2021, 2, 20, 1, 22, 22, 0, DateTimeKind.Utc), "dignissim pharetra. Nam ac nulla. In tincidunt congue turpis. In condimentum. Donec at arcu. Vestibulum ante ipsum", "en", new DateTime(2021, 8, 6, 15, 0, 33, 0, DateTimeKind.Utc), "Dictionary", "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2" },
                    { new Guid("d1852714-14aa-e991-d981-a7ea75a6a1e9"), new DateTime(2020, 6, 5, 20, 16, 3, 0, DateTimeKind.Utc), "placerat. Cras dictum ultricies ligula. Nullam enim. Sed nulla ante, iaculis nec, eleifend non, dapibus rutrum, justo. Praesent luctus. Curabitur egestas nunc sed libero. Proin sed turpis nec mauris blandit mattis. Cras eget nisi dictum augue malesuada malesuada. Integer id magna", "pt", new DateTime(2021, 4, 26, 7, 18, 28, 0, DateTimeKind.Utc), "Quiet User Text Grammar", "5881A9B3-912A-8F1A-C5FA-0855A0563E23" },
                    { new Guid("e43b711b-c5ca-ad30-4910-98eec7afd1a9"), new DateTime(2020, 11, 28, 14, 28, 55, 0, DateTimeKind.Utc), "tincidunt, neque vitae semper egestas, urna justo faucibus lectus, a sollicitudin orci sem eget massa. Suspendisse eleifend. Cras sed leo. Cras vehicula aliquet libero. Integer in magna. Phasellus dolor elit, pellentesque", "pt", new DateTime(2022, 2, 28, 2, 10, 57, 0, DateTimeKind.Utc), "Portuguese Study Quiet", "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167" },
                    { new Guid("e5b4bb88-f528-7535-f9be-d9f11be3db54"), new DateTime(2020, 10, 27, 17, 48, 41, 0, DateTimeKind.Utc), "Proin vel arcu", "ch", new DateTime(2021, 12, 23, 20, 54, 46, 0, DateTimeKind.Utc), "French Italian Portuguese Study Quiet", "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2" },
                    { new Guid("fcc0aa0a-d588-625a-64be-5e6dc8b06dbb"), new DateTime(2020, 11, 26, 22, 21, 8, 0, DateTimeKind.Utc), "dictum eleifend, nunc risus varius orci, in consequat enim diam vel arcu. Curabitur ut odio vel est tempor bibendum. Donec felis orci, adipiscing non, luctus sit amet, faucibus ut, nulla. Cras eu tellus eu augue porttitor interdum. Sed auctor", "fr", new DateTime(2022, 2, 23, 7, 32, 7, 0, DateTimeKind.Utc), "Quiet User Text Grammar", "BD696EC2-1BE1-FFB1-3FC1-883C2D247875" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("092bf87d-9de3-1bea-4fc5-f76294251979"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("0e275d7c-c765-a566-c091-3ac90c4f2a72"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("1280cd10-110e-d2c5-bbba-f6eea2c1b3e4"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("18d5f976-3c81-1711-ccd3-1ba2e7caa24c"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("1a44b87e-15c9-9ab6-2ba3-74623b8a1513"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("1f511e25-e470-6b8c-a283-c5f44fc61936"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("29717e54-7223-f856-cebe-5e5ee2f187b1"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("2a4424cc-2c1d-1309-a647-be27769a91c9"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("2a763e3c-0411-1fd6-2719-681eca29259e"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("336bbabc-4a14-b13c-2958-3c98335a354f"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("454177ec-70a4-e637-5d69-46202503c7f5"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("4942c531-e221-1d68-18d4-dc53b48eb51c"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("569698b2-60f5-4abe-4a8d-ec21259cb152"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("5db32645-4444-63aa-c1dc-c059789b215e"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("5df3869c-a2e7-88dc-4e4d-9c3fb0049ff6"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("81775da8-5104-21db-2b55-e1a1d8b4764a"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("85b91a36-6d3d-8578-1b4e-98389a57e1ac"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("913df528-fb15-529e-5017-dae1c74758d9"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("9963db07-6539-8a5e-aa87-b14c6dbd7631"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("ad56c6b4-4791-7982-4ac7-cca45243607e"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("bcac69c2-5a7d-74c9-e58a-792964b6ece9"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("d1852714-14aa-e991-d981-a7ea75a6a1e9"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("e43b711b-c5ca-ad30-4910-98eec7afd1a9"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("e5b4bb88-f528-7535-f9be-d9f11be3db54"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "DeckID",
                keyValue: new Guid("fcc0aa0a-d588-625a-64be-5e6dc8b06dbb"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e0c3809e-a8e9-4498-b93b-86f7de369b6c", "3f2b80c1-0c36-455a-af7d-b261f1183974" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0c8c5511-e136-4536-89a5-7d0dbd7dbeed", "4855ac84-b500-4371-83cd-ee443d63d5c0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "478ef678-6908-45ba-9735-f6bf29f1aa21", "4cc0593f-bdcd-4a6a-9ba5-38f6ba4218f6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "69a0f3ba-3062-4b52-909e-178e29da1f47", "7ef1c9fe-b9e3-48f0-8f5c-1c586eede900" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7d15889d-dee5-4ec5-b609-85115658fbec", "5c66bd78-e148-4998-a446-5eb5886820f6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bd509519-1f7c-4f68-a20a-8ba01d453ffb", "8e5e24dd-2a39-454d-8403-2c3bad6d4034" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "eb6bf882-9c93-4f81-a03d-dec55c603387", "7f9bce20-cdbc-4525-bbc2-491adf7dd45e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "27ed83c5-5c00-4766-915f-ee05e359a721", "e57aec56-d08d-46d8-bfe8-f56cb3cede7f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bc034eda-6360-415f-a89b-49b73ac03d39", "4ca7911e-3a64-42c0-90cd-c8da225a3e5d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "07b08efb-da00-4d3b-a67a-edcd28abf765", "3f197e73-5fc7-4bf9-92bb-a5234c6da760" });
        }
    }
}
