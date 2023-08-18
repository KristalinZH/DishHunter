using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DishHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixRestaurantPhoneMaxLenght : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09771dd1-74de-45d8-aa96-34e5e4614822"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5d13c532-43b7-4bb0-870a-6076125252d6"));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Restaurants",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7c7d3451-767b-419b-96ec-5df673cb54f1"), "e93f37e2-044c-4235-8574-c9c5bd77d2fb", "Administrator", "ADMINISTRATOR" },
                    { new Guid("c68aee11-94a0-4ce9-b090-e17777852cce"), "ebe7b523-78e2-4ce3-89eb-763ef1148484", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7c7d3451-767b-419b-96ec-5df673cb54f1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c68aee11-94a0-4ce9-b090-e17777852cce"));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Restaurants",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09771dd1-74de-45d8-aa96-34e5e4614822"), "bebbdb80-ff59-40a8-a0e0-e865378ac66f", "Administrator", "ADMINISTRATOR" },
                    { new Guid("5d13c532-43b7-4bb0-870a-6076125252d6"), "2e58a304-b0d6-4995-b235-1ec166c95721", "User", "USER" }
                });
        }
    }
}
