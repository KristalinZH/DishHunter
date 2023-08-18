using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DishHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeDbContextAndConfigFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7c7d3451-767b-419b-96ec-5df673cb54f1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c68aee11-94a0-4ce9-b090-e17777852cce"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("c421e5d4-85c6-4173-a17b-23c735028160"), "5462f444-c5fb-49b3-b0ee-c6ce02a15602", "User", "USER" },
                    { new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"), "99b9411d-689b-4dcc-adcf-70cc40a7d5fd", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "Settlements",
                columns: new[] { "Id", "IsActive", "Region", "SettlementName" },
                values: new object[] { 5248, true, "mANGALOVO", "Asparuhovo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c421e5d4-85c6-4173-a17b-23c735028160"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"));

            migrationBuilder.DeleteData(
                table: "Settlements",
                keyColumn: "Id",
                keyValue: 5248);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7c7d3451-767b-419b-96ec-5df673cb54f1"), "e93f37e2-044c-4235-8574-c9c5bd77d2fb", "Administrator", "ADMINISTRATOR" },
                    { new Guid("c68aee11-94a0-4ce9-b090-e17777852cce"), "ebe7b523-78e2-4ce3-89eb-763ef1148484", "User", "USER" }
                });
        }
    }
}
