using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FML4016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0004E137-13A5-5129-AA50-5A856A2A86F7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cd111b1c-373f-4b0b-a32e-dd6e447acb3e", "3bcd006e-d4d0-41ed-91d5-1d6ff5fb5228" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5881A9B3-912A-8F1A-C5FA-0855A0563E23",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "15b5c093-bbfd-465e-90fc-1a977eeb254d", "dcd33d0d-c7ea-4a9c-bd60-fd60128c0afa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62DAAEBC-F2F5-4D16-533A-AD176D7EA7B7",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "38717423-1d55-4665-9ba2-94e1da274c05", "66102e4b-8701-47fa-a215-19282f0824dd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6BBD068B-B258-EB0A-2132-BEB9BAEA886E",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bbfc373b-7f3c-4573-a4ee-82c45f40051d", "eac636b0-8675-4694-a6b0-8bffd05240a6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9CBF5FEE-EE6E-5F1D-3E53-2389AD983EA2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "744843ba-73d7-4327-90c5-174ee7154be3", "52e61dcf-351c-4ccc-a353-235c0ba3ecf6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9F87EEA2-25CB-13B8-13B7-CA1A69CA68B4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "083810c9-0a66-456b-9c02-f876c01cdfcd", "55a9098d-3904-4d50-b466-0fd61d7b6f0a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9FBC287A-C113-EBA3-526F-4AE7CEACBDE8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ca1cbd4f-1e15-471b-957d-dc1fb4a81228", "c5836478-822a-49c6-9c16-ca75b40c0623" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B4553CAB-7997-1DE5-F7E3-90A7BC1DA167",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bcbd2991-fd63-4043-897a-af874be381b6", "744f25c6-61a3-4802-a0a2-d3f86342aacd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BD696EC2-1BE1-FFB1-3FC1-883C2D247875",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d1612863-c054-485d-b648-d79b132e5326", "badaa919-525c-46c0-81c1-bf89629561c5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "E4C3E116-C71A-7B3B-CA32-159AE97C8456",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e5ba0eff-e169-4813-a2af-34fb74b48b54", "abe175ce-a34c-4b33-8571-f1b182d8b69b" });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "FlashcardID", "Answer", "BackContentLayout", "Content1", "Content2", "Content3", "Content4", "Content5", "Content6", "CreationDate", "DeckId", "DueDate", "FrontContentLayout", "LastUpdated", "Level" },
                values: new object[,]
                {
                    { new Guid("1c710e71-b53e-4c7d-9f1d-080b57909044"), "Tarte", 4, "https://target.scene7.com/is/image/Target/GUEST_d29e72d0-b1cd-4cfd-8c8b-f2a200fd7193?wid=488&hei=488&fmt=pjpeg", "Pariatur irure do nostrud sit. Consectetur elit excepteur velit commodo qui cillum occaecat consectetur cupidatat et reprehenderit sit. Aute nulla occaecat cupidatat. Occaecat excepteur laborum deserunt exercitation sit ea proident aliquip. Incididunt magna laboris in pariatur fugiat do exercitation pariatur non elit mollit ut proident. Elit dolore est deserunt laboris sint mollit ex deserunt. Proident ad amet minim magna magna pariatur ad proident enim amet Lorem. Magna proident officia cupidatat magna.", "", "https://audio12.forvo.com/audios/mp3/u/n/un_9523171_118_7665857.mp3", "Sunt anim laborum Lorem veniam excepteur veniam aute consectetur proident proident anim veniam nulla enim. Et eiusmod incididunt aute ex in sunt. Magna incididunt culpa mollit incididunt exercitation duis deserunt consectetur ad eu dolor quis. Eiusmod laborum ullamco mollit officia cupidatat tempor incididunt dolor consequat nisi ea officia nostrud elit deserunt. Sunt anim sunt sint cupidatat sint ipsum qui labore cillum consectetur exercitation proident. Consectetur ad incididunt cupidatat ut voluptate.", "https://images.contentstack.io/v3/assets/blt731acb42bb3d1659/blt48f811476e162ed0/620c15764ae5ae6845c6b0c9/LOL_Homepage_Modal_(1680x650)_(1).jpg", new DateTime(2020, 1, 15, 23, 16, 8, 0, DateTimeKind.Utc), new Guid("e43b711b-c5ca-ad30-4910-98eec7afd1a9"), new DateTime(2020, 3, 19, 23, 16, 8, 0, DateTimeKind.Utc), "VERTICAL_SPLIT", new DateTime(2020, 1, 15, 23, 16, 8, 0, DateTimeKind.Utc), 8 },
                    { new Guid("3d7978b1-c270-4ae7-ade5-38becf5770e9"), "Tarte", 4, "<p><b>arbol</b></p><p>Category: Noun</p><p>Spelling: </p><p>Definitions:</p><ul><li>Planta de tronco leñoso, grueso y elevado que se ramifica a cierta altura del suelo formando la copa</li><li>Cuadro descriptivo que representa de forma gráfica las relaciones que tienen los elementos de un conjunto o las variaciones de un fenómeno</li><li>Barra fija o giratoria que en una máquina sirve para soportar piezas rotativas o para transmitir fuerza motriz de unos órganos a otros</li><li>Barra o eje que se usa en posición vertical y sirve de apoyo a una estructura que se desarrolla alrededor del mismo, como el árbol de una escalera de caracol</li><li>Madero que junto con otros se coloca perpendicularmente a la quilla de una embarcación y está destinado a sujetar las velas</li><li>Eje del órgano (instrumento musical) que, movido a voluntad del ejecutante, hace que suene o deje de sonar un registro</li><li>Punzón que usan los relojeros para perforar el metal y que tiene el mango de madera y la punta de acero</li><li>Pieza de hierro en la parte superior del husillo de la prensa de imprimir</li></ul><p>Examples:</p><ul><li>el Ministerio de Medio Ambiente tiene previsto plantar un millón de árboles en el próximo año</li><li>diagrama en árbol</li></ul>", "https://target.scene7.com/is/image/Target/GUEST_b3e2da64-ad54-48b5-9dc7-50071ea5075d?wid=488&hei=488&fmt=pjpeg", "", "https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6463/6463555_sd.jpg", "https://m.media-amazon.com/images/I/81rz6kuxieL._AC_SL1500_.jpg", "Labore et minim enim Lorem. Sint cupidatat adipisicing enim aliquip incididunt est anim laborum officia. Lorem laborum proident amet nisi aute culpa. Id labore irure duis nisi id magna velit. Nulla pariatur anim adipisicing magna esse anim in officia cupidatat exercitation. Irure minim est exercitation mollit exercitation veniam aute consequat. Sit tempor irure ut. Adipisicing duis deserunt consequat deserunt sunt Lorem ipsum occaecat laborum do aliqua elit.", new DateTime(2020, 1, 31, 20, 47, 48, 0, DateTimeKind.Utc), new Guid("85b91a36-6d3d-8578-1b4e-98389a57e1ac"), new DateTime(2020, 4, 4, 20, 47, 48, 0, DateTimeKind.Utc), "VERTICAL_SPLIT", new DateTime(2020, 1, 31, 20, 47, 48, 0, DateTimeKind.Utc), 8 },
                    { new Guid("4956fb2a-8805-40e2-a605-20c43d44b509"), "nuage", 0, "Reprehenderit proident ex amet ea voluptate non in duis culpa. Nulla duis id eu fugiat nostrud pariatur velit do consectetur dolore cillum nulla cillum cillum. Voluptate esse qui et dolore commodo laborum aliquip sunt anim ex ut. Aute cupidatat reprehenderit velit id tempor veniam consectetur sunt ad qui nisi ea deserunt. Cupidatat ad ea in. Nisi reprehenderit laboris aute quis ipsum minim consequat exercitation voluptate adipisicing aliquip magna adipisicing.", "", "", "https://audio12.forvo.com/audios/mp3/l/i/li_9002112_133_543430_266179.mp3", "", "", new DateTime(2020, 1, 30, 8, 25, 43, 0, DateTimeKind.Utc), new Guid("29717e54-7223-f856-cebe-5e5ee2f187b1"), new DateTime(2020, 1, 31, 8, 25, 43, 0, DateTimeKind.Utc), "SINGLE_BLOCK", new DateTime(2020, 1, 30, 8, 25, 43, 0, DateTimeKind.Utc), 1 },
                    { new Guid("57c914a6-6c2e-4de0-a336-c2b8be66b3b6"), "vecchia signora", 2, "https://audio12.forvo.com/audios/mp3/l/i/li_9002112_133_543430_266179.mp3", "Magna cillum aute ad nisi. Cillum deserunt adipisicing adipisicing reprehenderit culpa. Non amet occaecat fugiat do labore veniam. Do fugiat magna occaecat Lorem nostrud mollit tempor reprehenderit.", "<p><b>car</b></p><p>Category: Noun</p><p>Spelling: kɑː</p><p>Definitions:</p><ul><li>a four-wheeled road vehicle that is powered by an engine and is able to carry a small number of people</li></ul><p>Examples:</p><ul><li>she drove up in a car</li><li>we're going by car</li></ul>", "Nulla irure amet anim. Incididunt ullamco sunt quis. Sit tempor ad velit occaecat ex. Quis in aliquip cupidatat esse reprehenderit ipsum veniam irure aliquip do commodo cillum amet deserunt voluptate. Officia cillum culpa id veniam et ipsum amet consequat deserunt ea. Ex proident mollit esse enim dolor sunt ea officia labore aute.", "<p><b>love</b></p><p>Category: Noun</p><p>Spelling: ləv</p><p>Definitions:</p><ul><li>an intense feeling of deep affection</li><li>a great interest and pleasure in something</li><li>a person or thing that one loves</li><li>(in tennis, squash, and some other sports) a score of zero; nil</li></ul><p>Examples:</p><ul><li>babies fill parents with feelings of love</li><li>their love for their country</li><li>his love for football</li><li>we share a love of music</li><li>she was the love of his life</li><li>their two great loves are tobacco and whiskey</li><li>love fifteen</li><li>he was down two sets to love</li></ul><p>Category: Verb</p><p>Spelling: ləv</p><p>Definitions:</p><ul><li>feel deep affection for (someone)</li><li>like or enjoy very much</li></ul><p>Examples:</p><ul><li>he loved his sister dearly</li><li>there were four memorial pages set up by her friends in honor of Phoebe, saying how much they loved and missed her</li><li>I just love dancing</li><li>I'd love a cup of tea</li><li>I love this job</li><li>they love to play golf</li></ul>", "https://audio12.forvo.com/audios/mp3/l/i/li_9002112_133_543430_266179.mp3", new DateTime(2020, 1, 31, 17, 42, 15, 0, DateTimeKind.Utc), new Guid("9963db07-6539-8a5e-aa87-b14c6dbd7631"), new DateTime(2020, 3, 20, 17, 42, 15, 0, DateTimeKind.Utc), "TRIPLE_BLOCK", new DateTime(2020, 1, 31, 17, 42, 15, 0, DateTimeKind.Utc), 7 },
                    { new Guid("5b740554-e348-4327-a5a2-788c91c7b2d8"), "macchina", 1, "https://target.scene7.com/is/image/Target/GUEST_d29e72d0-b1cd-4cfd-8c8b-f2a200fd7193?wid=488&hei=488&fmt=pjpeg", "Esse consequat ex in velit non cupidatat dolor consectetur aute consequat sunt. Enim tempor consectetur duis ullamco ipsum laboris proident mollit enim culpa cupidatat velit consectetur. Excepteur sit et eu veniam fugiat ipsum voluptate elit anim. Dolore irure mollit est nulla. Irure laboris labore velit et do irure ipsum labore esse ullamco aute magna sit reprehenderit. Culpa eu minim consequat labore ut. Sint sint consequat esse dolore aute ex consequat excepteur qui veniam veniam.", "", "Aliquip nisi dolore veniam irure consequat ad laborum commodo sit. Ad irure id est occaecat reprehenderit aliquip elit non. Eiusmod dolor veniam est in ea proident consectetur. Id anim adipisicing incididunt Lorem ad ut Lorem magna nostrud officia duis cupidatat aliquip. Culpa ex laboris consectetur tempor sit eiusmod adipisicing occaecat magna qui qui. Officia fugiat consequat exercitation proident reprehenderit officia cupidatat eu. Irure officia adipisicing consequat excepteur cillum aliqua reprehenderit Lorem proident exercitation consequat pariatur ea occaecat do.", "https://audio12.forvo.com/audios/mp3/4/r/4r_20_74_136_1.mp3", "", new DateTime(2020, 1, 10, 14, 55, 30, 0, DateTimeKind.Utc), new Guid("e43b711b-c5ca-ad30-4910-98eec7afd1a9"), new DateTime(2020, 4, 19, 14, 55, 30, 0, DateTimeKind.Utc), "VERTICAL_SPLIT", new DateTime(2020, 1, 10, 14, 55, 30, 0, DateTimeKind.Utc), 10 },
                    { new Guid("68c70248-d828-4ee0-b032-fb3efa850e73"), "el pueblo", 2, "Aute nulla dolore reprehenderit aute officia elit consequat magna dolor ut duis. Consectetur et eiusmod dolore consequat veniam aute duis pariatur commodo. Cupidatat exercitation amet ullamco et cupidatat elit. Irure labore commodo mollit elit aute tempor. Pariatur ex et magna consectetur Lorem. Consequat nostrud amet quis. Id esse consectetur reprehenderit esse dolore veniam fugiat fugiat mollit id tempor.", "https://audio12.forvo.com/audios/mp3/p/v/pv_8979922_49_607322_1.mp3", "", "Duis occaecat adipisicing enim qui fugiat do dolore deserunt. Do pariatur velit amet fugiat tempor dolor aliqua pariatur excepteur dolore exercitation elit. Duis aliqua proident sit consequat ad adipisicing consectetur. Est non incididunt culpa officia elit velit pariatur nostrud qui cillum proident Lorem tempor. Officia irure dolor adipisicing commodo Lorem sit velit veniam irure. Pariatur ipsum commodo amet Lorem consequat. Deserunt proident Lorem et dolor. Ullamco laboris aute elit irure.", "https://target.scene7.com/is/image/Target/GUEST_d29e72d0-b1cd-4cfd-8c8b-f2a200fd7193?wid=488&hei=488&fmt=pjpeg", "<p><b>arbol</b></p><p>Category: Noun</p><p>Spelling: </p><p>Definitions:</p><ul><li>Planta de tronco leñoso, grueso y elevado que se ramifica a cierta altura del suelo formando la copa</li><li>Cuadro descriptivo que representa de forma gráfica las relaciones que tienen los elementos de un conjunto o las variaciones de un fenómeno</li><li>Barra fija o giratoria que en una máquina sirve para soportar piezas rotativas o para transmitir fuerza motriz de unos órganos a otros</li><li>Barra o eje que se usa en posición vertical y sirve de apoyo a una estructura que se desarrolla alrededor del mismo, como el árbol de una escalera de caracol</li><li>Madero que junto con otros se coloca perpendicularmente a la quilla de una embarcación y está destinado a sujetar las velas</li><li>Eje del órgano (instrumento musical) que, movido a voluntad del ejecutante, hace que suene o deje de sonar un registro</li><li>Punzón que usan los relojeros para perforar el metal y que tiene el mango de madera y la punta de acero</li><li>Pieza de hierro en la parte superior del husillo de la prensa de imprimir</li></ul><p>Examples:</p><ul><li>el Ministerio de Medio Ambiente tiene previsto plantar un millón de árboles en el próximo año</li><li>diagrama en árbol</li></ul>", new DateTime(2020, 1, 15, 7, 5, 31, 0, DateTimeKind.Utc), new Guid("9963db07-6539-8a5e-aa87-b14c6dbd7631"), new DateTime(2020, 1, 19, 7, 5, 31, 0, DateTimeKind.Utc), "VERTICAL_SPLIT", new DateTime(2020, 1, 15, 7, 5, 31, 0, DateTimeKind.Utc), 2 },
                    { new Guid("7a3f13fb-ac61-4e24-b80d-f5593f788826"), "", 0, "https://audio12.forvo.com/audios/mp3/u/n/un_9523171_118_7665857.mp3", "Anim ea laborum aliqua nulla cillum dolore mollit ad reprehenderit adipisicing sit nostrud esse aliquip amet. Cupidatat ad voluptate eu laboris ea tempor deserunt sint id. Anim laboris ullamco tempor excepteur occaecat aliqua veniam culpa magna nisi aliquip. Aliquip voluptate ullamco eiusmod deserunt aute quis cupidatat voluptate id do fugiat. Reprehenderit aliquip fugiat non. Sint deserunt labore nisi exercitation aliqua velit ullamco eiusmod qui esse ex et aliquip laborum.", "", "https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6463/6463555_sd.jpg", "", "", new DateTime(2020, 1, 3, 22, 34, 2, 0, DateTimeKind.Utc), new Guid("81775da8-5104-21db-2b55-e1a1d8b4764a"), new DateTime(2020, 1, 3, 22, 34, 2, 0, DateTimeKind.Utc), "VERTICAL_SPLIT", new DateTime(2020, 1, 3, 22, 34, 2, 0, DateTimeKind.Utc), 0 },
                    { new Guid("8d0af6e4-c06d-4953-bde0-3c67f4517954"), "Gelsenkirchen", 4, "Ex eu exercitation eu labore culpa. Cillum dolore sunt nostrud aute sint. Aliqua exercitation ad ipsum veniam ea proident. Elit consectetur do do do laboris aute sint ad qui ullamco consequat officia aliquip.", "<p><b>calcio</b></p><p>Category: noun</p><p>Spelling: ˈkaltʃo</p><p>Definitions:</p><ul><li>impugnatura della pistola</li><li>parte del fucile che si appoggia alla spalla</li></ul><p>Examples:</p><ul><li>carabina con calcio in legno</li><li>L'aggressore lo colpì con il calcio della pistola.</li></ul><p>Category: noun</p><p>Spelling: ˈkaltʃo</p><p>Definitions:</p><ul><li>colpo dato con il piede</li><li>colpo dato con la zampa da animali forniti di zoccoli</li><li>il gioco del pallone</li><li>tiro effettuato colpendo col piede il pallone</li></ul><p>Examples:</p><ul><li>dare un calcio</li><li>prendere a calci</li><li>Il mulo tira calci se infastidito.</li><li>una partita di calcio</li><li>la nazionale di calcio</li><li>calcio d'angolo</li><li>calcio di punizione</li></ul><p>Category: noun</p><p>Spelling: ˈkaltʃo</p><p>Definitions:</p><ul><li>elemento chimico di colore bianco</li></ul><p>Examples:</p><ul><li>Il latte è ricco di calcio.</li></ul>", "https://audio12.forvo.com/audios/mp3/6/o/6o_9014612_41_617305_1.mp3", "Commodo aliquip magna cupidatat deserunt id deserunt do sint officia. Mollit est labore exercitation culpa nostrud ullamco ex. Consequat mollit nisi irure elit mollit nisi cupidatat esse dolor officia esse pariatur magna proident. Non est sit exercitation commodo minim ad anim velit consequat dolore nulla do magna. Adipisicing aliquip fugiat ad sunt in velit. Dolor amet nostrud culpa culpa id ipsum culpa reprehenderit velit sint.", "<p><b>car</b></p><p>Category: Noun</p><p>Spelling: kɑː</p><p>Definitions:</p><ul><li>a four-wheeled road vehicle that is powered by an engine and is able to carry a small number of people</li></ul><p>Examples:</p><ul><li>she drove up in a car</li><li>we're going by car</li></ul>", "Est pariatur et reprehenderit ipsum aliquip aute. Ea dolore sunt officia proident proident aliquip irure voluptate non duis. Ut commodo esse do ut officia ea do nulla officia eu qui amet officia commodo amet. Fugiat commodo deserunt qui incididunt occaecat reprehenderit cupidatat aute ut in. Irure aliqua commodo eu proident nisi fugiat voluptate excepteur labore enim reprehenderit officia veniam aliquip incididunt. Irure incididunt ea ullamco magna occaecat culpa qui. Amet sit adipisicing eu ex minim laborum eiusmod dolor proident. Dolore deserunt ex esse voluptate et est voluptate duis ipsum sit sint.", new DateTime(2020, 1, 6, 18, 20, 21, 0, DateTimeKind.Utc), new Guid("5db32645-4444-63aa-c1dc-c059789b215e"), new DateTime(2020, 1, 31, 18, 20, 21, 0, DateTimeKind.Utc), "FULL_CARD", new DateTime(2020, 1, 6, 18, 20, 21, 0, DateTimeKind.Utc), 5 },
                    { new Guid("9ebc936b-42db-4798-8ffe-f1e10a06dd46"), "el pueblo", 4, "https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6463/6463555_sd.jpg", "https://m.media-amazon.com/images/I/81rz6kuxieL._AC_SL1500_.jpg", "Consequat amet ad fugiat dolore nostrud qui ad qui. Cupidatat consequat dolor in magna duis. Aliqua anim nulla reprehenderit sunt tempor. Ex laboris proident commodo consequat eiusmod ea elit minim ex. Et exercitation est veniam id sint cupidatat dolor eu eiusmod culpa aute aute excepteur id culpa. Nulla dolore voluptate nostrud incididunt adipisicing exercitation ad quis. Deserunt sint irure laborum mollit nostrud laboris do non magna. Do magna pariatur commodo reprehenderit nostrud ipsum magna do consequat laborum Lorem.", "Veniam eiusmod velit irure cupidatat nostrud aliqua id. Amet voluptate occaecat velit elit sit esse reprehenderit labore amet. Aliqua cillum sunt sit aute quis mollit excepteur magna qui consectetur dolor aute sunt. Aliquip et aliqua eiusmod qui tempor minim eu est culpa commodo do consequat dolore. Excepteur amet ad sint aliquip nostrud Lorem pariatur cupidatat incididunt cupidatat. Laborum ullamco officia fugiat laborum aliqua fugiat pariatur nostrud sint ea voluptate qui dolor Lorem commodo. Sint et ex ipsum proident eiusmod qui voluptate labore consectetur labore aute culpa quis tempor. Voluptate nostrud eiusmod Lorem labore cupidatat id ad.", "Duis nisi ad commodo incididunt culpa eiusmod cillum. Irure nisi et laboris aliquip labore cupidatat dolore nostrud pariatur consequat id magna anim est est. Consectetur veniam eu commodo adipisicing aliquip aliquip aliquip velit est incididunt adipisicing. Pariatur laboris quis velit voluptate commodo nulla duis laboris eu velit. Quis elit laborum irure velit. Veniam est deserunt qui.", "https://audio12.forvo.com/audios/mp3/p/v/pv_8979922_49_607322_1.mp3", new DateTime(2020, 1, 4, 22, 9, 18, 0, DateTimeKind.Utc), new Guid("5df3869c-a2e7-88dc-4e4d-9c3fb0049ff6"), new DateTime(2020, 2, 22, 22, 9, 18, 0, DateTimeKind.Utc), "TRIPLE_BLOCK", new DateTime(2020, 1, 4, 22, 9, 18, 0, DateTimeKind.Utc), 7 },
                    { new Guid("d12ae132-f96a-436a-8374-15402ffbd45d"), "Tarte", 3, "Ad est dolor nisi esse esse. Sit Lorem ex in sunt sit aliquip. Amet mollit in nulla consequat anim deserunt ea Lorem cupidatat. Aute esse irure aute tempor est quis qui culpa esse nostrud. Ea ea magna do aliquip duis incididunt tempor proident. Sunt ipsum tempor enim.", "https://audio12.forvo.com/audios/mp3/l/i/li_9002112_133_543430_266179.mp3", "", "Sit nostrud do exercitation irure adipisicing. Enim deserunt do laboris ad cillum elit. Fugiat anim ullamco incididunt. Sunt pariatur nostrud dolore anim duis nostrud excepteur dolor ipsum qui occaecat ea irure aute tempor. Id officia aliqua nulla voluptate Lorem esse adipisicing cillum eu sint eu eu ut do est.", "https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6463/6463555_sd.jpg", "", new DateTime(2020, 1, 14, 11, 54, 56, 0, DateTimeKind.Utc), new Guid("e43b711b-c5ca-ad30-4910-98eec7afd1a9"), new DateTime(2020, 2, 7, 11, 43, 35, 0, DateTimeKind.Utc), "HORIZONTAL_SPLIT", new DateTime(2020, 1, 29, 11, 43, 35, 0, DateTimeKind.Utc), 3 },
                    { new Guid("e83966e7-af40-4892-aa74-52c3b1ad77f8"), "", 0, "Voluptate ea reprehenderit cillum deserunt tempor cillum. Sunt id fugiat incididunt anim eiusmod nulla aliquip elit exercitation dolor ex amet. Ipsum commodo laborum voluptate reprehenderit culpa minim dolore cupidatat dolore culpa enim dolore mollit enim magna. Non adipisicing amet labore aliquip laborum mollit magna velit est dolor sunt aliquip esse. Consequat mollit dolor proident Lorem. Eiusmod ipsum nulla amet fugiat officia amet laborum aute reprehenderit minim veniam in adipisicing labore do.", "Consequat laboris commodo fugiat reprehenderit adipisicing sint pariatur ut nisi tempor. Esse ut nulla culpa ad. Incididunt nostrud qui proident ad ut amet adipisicing enim quis proident. Aute dolore consequat ad velit Lorem nulla dolore tempor eu laboris et.", "Reprehenderit eu ut officia laborum officia aliqua eu ea mollit aliqua reprehenderit nostrud irure. Exercitation adipisicing mollit sunt qui magna deserunt elit incididunt aliquip duis ut. Duis velit in nostrud nulla qui fugiat velit nostrud adipisicing. Exercitation id nisi reprehenderit culpa reprehenderit aliqua duis ea sit cillum exercitation.", "<p><b>car</b></p><p>Category: Noun</p><p>Spelling: kɑː</p><p>Definitions:</p><ul><li>a four-wheeled road vehicle that is powered by an engine and is able to carry a small number of people</li></ul><p>Examples:</p><ul><li>she drove up in a car</li><li>we're going by car</li></ul>", "", "", new DateTime(2020, 1, 11, 16, 3, 36, 0, DateTimeKind.Utc), new Guid("e43b711b-c5ca-ad30-4910-98eec7afd1a9"), new DateTime(2020, 4, 1, 16, 3, 36, 0, DateTimeKind.Utc), "TRIPLE_BLOCK", new DateTime(2020, 1, 11, 16, 3, 36, 0, DateTimeKind.Utc), 9 },
                    { new Guid("e956e210-1965-4d3b-bf68-b6977733cdb7"), "kill", 0, "https://target.scene7.com/is/image/Target/GUEST_b3e2da64-ad54-48b5-9dc7-50071ea5075d?wid=488&hei=488&fmt=pjpeg", "https://audio12.forvo.com/audios/mp3/4/r/4r_20_74_136_1.mp3", "https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6463/6463555_sd.jpg", "In mollit eu eiusmod excepteur dolore adipisicing sit commodo ipsum in fugiat sint. Incididunt et proident culpa magna cupidatat sit adipisicing pariatur Lorem dolore. Officia reprehenderit id qui elit excepteur reprehenderit sit ad dolore. Quis eu esse non eu veniam nisi ea. Sunt do elit officia dolor cupidatat nisi ut. Culpa et exercitation id quis enim consequat in aliquip qui deserunt minim velit ad. Commodo dolor proident exercitation amet minim et.", "", "", new DateTime(2020, 1, 7, 20, 18, 3, 0, DateTimeKind.Utc), new Guid("0e275d7c-c765-a566-c091-3ac90c4f2a72"), new DateTime(2020, 2, 1, 20, 18, 3, 0, DateTimeKind.Utc), "FULL_CARD", new DateTime(2020, 1, 7, 20, 18, 3, 0, DateTimeKind.Utc), 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("1c710e71-b53e-4c7d-9f1d-080b57909044"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("3d7978b1-c270-4ae7-ade5-38becf5770e9"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("4956fb2a-8805-40e2-a605-20c43d44b509"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("57c914a6-6c2e-4de0-a336-c2b8be66b3b6"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("5b740554-e348-4327-a5a2-788c91c7b2d8"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("68c70248-d828-4ee0-b032-fb3efa850e73"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("7a3f13fb-ac61-4e24-b80d-f5593f788826"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("8d0af6e4-c06d-4953-bde0-3c67f4517954"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("9ebc936b-42db-4798-8ffe-f1e10a06dd46"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("d12ae132-f96a-436a-8374-15402ffbd45d"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("e83966e7-af40-4892-aa74-52c3b1ad77f8"));

            migrationBuilder.DeleteData(
                table: "Flashcards",
                keyColumn: "FlashcardID",
                keyValue: new Guid("e956e210-1965-4d3b-bf68-b6977733cdb7"));

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
        }
    }
}
