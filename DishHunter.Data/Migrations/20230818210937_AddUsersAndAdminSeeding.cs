using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DishHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndAdminSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c421e5d4-85c6-4173-a17b-23c735028160"),
                column: "ConcurrencyStamp",
                value: "2d3358c4-45d4-4dd3-bd0e-3e8ba1c0cd67");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                column: "ConcurrencyStamp",
                value: "8767b0cb-6817-4974-872b-8961cff922c8");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RestaurantOwnerId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("781fe215-36fe-4183-9844-ab5685cc8c24"), 0, "418e37ac-38a1-4a93-aee8-6ae50a59624d", "admin@email.com", false, "Administrator", "Administrator", false, null, "ADMIN@EMAIL.COM", "ADMINISTRATOR", "AQAAAAEAACcQAAAAEFvdB3RwE8w7xuW5gsmLd1rtusryw4GJtfmPIsulMxOLnYA/iJBB60WMVmFrlRU0hQ==", "+359883333444", false, null, "aa3df9f7-74d9-4d50-99d0-82a6196361b0", false, "Administrator" },
                    { new Guid("a39c8167-04c4-4235-9fa2-08162c00864d"), 0, "53bc0e21-a00d-483b-aa84-3d74947b230e", "misho@email.com", false, "Misho", "Mishov", false, null, "MISHO@EMAIL.COM", "MISHOTHEUSER", "AQAAAAEAACcQAAAAEKAFJD6fqWwfMVgvydVlCtF0Wnbcf7RuZECJDcroBpfDONsX1c/uFbhPvD5HgX/QXA==", null, false, null, "395906be-b39c-4b6b-9581-4c2ce6b392b0", false, "MishoTheUser" }
                });

            migrationBuilder.InsertData(
                table: "RestaurantOwners",
                columns: new[] { "Id", "IsActive", "UserId" },
                values: new object[] { new Guid("10a2102e-f116-4b81-b0a0-32b0d1022cb9"), true, new Guid("aadb31cc-2d98-4864-84f7-127ea6097123") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"), new Guid("781fe215-36fe-4183-9844-ab5685cc8c24") });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RestaurantOwnerId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("aadb31cc-2d98-4864-84f7-127ea6097123"), 0, "44d72def-91b5-4c39-b1ab-78d3f57b3415", "pesho@email.com", false, "Pesho", "Peshov", false, null, "Pesho@EMAIL.COM", "PESHOTHEOWNER", "AQAAAAEAACcQAAAAEFhm3aHnG2f+Xml67gSHRbStTQ/Hh5dryNZ26ACuj2sjpwDa7C0GnSg22MI79Nc/CA==", "+359884444333", false, new Guid("10a2102e-f116-4b81-b0a0-32b0d1022cb9"), "e2654e3c-cba3-45a0-87e0-4a49f5b2225d", false, "PeshoTheOwner" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"), new Guid("781fe215-36fe-4183-9844-ab5685cc8c24") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a39c8167-04c4-4235-9fa2-08162c00864d"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aadb31cc-2d98-4864-84f7-127ea6097123"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("781fe215-36fe-4183-9844-ab5685cc8c24"));

            migrationBuilder.DeleteData(
                table: "RestaurantOwners",
                keyColumn: "Id",
                keyValue: new Guid("10a2102e-f116-4b81-b0a0-32b0d1022cb9"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c421e5d4-85c6-4173-a17b-23c735028160"),
                column: "ConcurrencyStamp",
                value: "5462f444-c5fb-49b3-b0ee-c6ce02a15602");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                column: "ConcurrencyStamp",
                value: "99b9411d-689b-4dcc-adcf-70cc40a7d5fd");
        }
    }
}
