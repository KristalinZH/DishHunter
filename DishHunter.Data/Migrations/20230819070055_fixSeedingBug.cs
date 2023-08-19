using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DishHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixSeedingBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c421e5d4-85c6-4173-a17b-23c735028160"),
                column: "ConcurrencyStamp",
                value: "fed8bbf1-bf16-4331-9c92-9e10c0a55520");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                column: "ConcurrencyStamp",
                value: "10c09048-ba68-4e46-9759-c0edc90143b6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8a4d1997-8ace-42ba-aac5-1fe005eabd99"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8417dc3d-d264-4ec8-8c45-a4abbd8d87d0", "AQAAAAEAACcQAAAAEEez196xAczVIZMyXxoCMZeKCIQIkv1qVP416Gx1XmbEu/zapJQaHr+IPONIkxv9pA==", "4c59519b-0e7f-48eb-82f4-6b94ab859040" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9e9d933d-973a-433a-ada2-19e4a7d4a509"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e73d99a8-602b-4225-9cb1-d1c1f5e7ddda", "AQAAAAEAACcQAAAAELEryCvhmgd6eXHBJVQBPD52xaCXmXjPyxgTrjZrStyolxjqZ7ZvRkSxl/2sijJeVw==", "13f45043-d2cd-45ac-a05f-76758a1b08ac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b49d1805-e143-47ed-9b72-7761e20d6c88"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d66b7678-abe9-4083-aa00-54966be4a563", "AQAAAAEAACcQAAAAECfYKYh6pG3zKzgXZhycjgIawJyxLkLBZU5rfTm+vim9o6LzQJarj3Mo5wxTIz9FzA==", "dece7810-a7dd-4736-9487-24494ae58e9b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d5b353de-0b76-4168-ba6f-bcfcdb7e3029"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a733666b-fb67-4d2e-aa48-e825070282ce", "AQAAAAEAACcQAAAAELp21Vl5rbDLrtATPgLTWt5MolkJrYKqUKeYXZj6bONcSPByFGCIEnuvkIIykWqbYw==", "4302d1eb-b446-4854-be2d-6ac3b07f54d7" });

            migrationBuilder.UpdateData(
                table: "RestaurantOwners",
                keyColumn: "Id",
                keyValue: new Guid("62152f86-525b-454f-92c8-108cea75c239"),
                column: "UserId",
                value: new Guid("b49d1805-e143-47ed-9b72-7761e20d6c88"));

            migrationBuilder.UpdateData(
                table: "RestaurantOwners",
                keyColumn: "Id",
                keyValue: new Guid("62152f86-525b-454f-92c8-108cea75c240"),
                column: "UserId",
                value: new Guid("9e9d933d-973a-433a-ada2-19e4a7d4a509"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c421e5d4-85c6-4173-a17b-23c735028160"),
                column: "ConcurrencyStamp",
                value: "03fd43f9-efe3-41e7-9a19-e87f416b23d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                column: "ConcurrencyStamp",
                value: "104ebff7-19be-4375-aeb0-c416fe69e500");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8a4d1997-8ace-42ba-aac5-1fe005eabd99"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "60d7c3c6-7a14-436f-9649-91511f802076", "AQAAAAEAACcQAAAAEOc/TXIp39g1hBAzFBnXFAmfdGtAqCT7fG7vNozx8a9vj1jlTGwZIAWS6r1RlBEPXA==", "d0906cc3-c708-4525-8553-773b401457a4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9e9d933d-973a-433a-ada2-19e4a7d4a509"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ca1dbd5-98a7-439e-99cd-2bb49f63a4da", "AQAAAAEAACcQAAAAEGixZ7lWPEDqCZWRmoSZcpaNqx/5vLRAQivJZy9Ohdzu3eEDFh2+IGPQ8SbiJRSOVA==", "d5a30d9d-498f-470e-b912-ae70ede92502" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b49d1805-e143-47ed-9b72-7761e20d6c88"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "87d30734-08db-4e5c-a0a0-3863d9ce4f44", "AQAAAAEAACcQAAAAEPoOWePcGDBvO+VQ1o+kb0Ky2DG1+KVeKFHFnmyyd8nTbJAH83ZN1J0b08PA0vubIA==", "624a8235-441e-46c4-80fb-c7a27c081ebe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d5b353de-0b76-4168-ba6f-bcfcdb7e3029"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cdb03d71-9a23-4c3f-b3fa-7953053c993b", "AQAAAAEAACcQAAAAEN2vCxeJbziGYO0UuaaydM1+4hGGPd3RcI5/pencgozGUjkveUYXMX4U32yqunfVzg==", "eaad52a0-d9d3-4880-bec0-6af9cf670abf" });

            migrationBuilder.UpdateData(
                table: "RestaurantOwners",
                keyColumn: "Id",
                keyValue: new Guid("62152f86-525b-454f-92c8-108cea75c239"),
                column: "UserId",
                value: new Guid("aadb31cc-2d98-4864-84f7-127ea6097123"));

            migrationBuilder.UpdateData(
                table: "RestaurantOwners",
                keyColumn: "Id",
                keyValue: new Guid("62152f86-525b-454f-92c8-108cea75c240"),
                column: "UserId",
                value: new Guid("aadb31cc-2d98-4864-84f7-127ea609712a"));
        }
    }
}
