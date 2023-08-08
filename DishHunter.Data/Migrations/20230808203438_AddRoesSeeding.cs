using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DishHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09771dd1-74de-45d8-aa96-34e5e4614822"), "bebbdb80-ff59-40a8-a0e0-e865378ac66f", "Administrator", "ADMINISTRATOR" },
                    { new Guid("5d13c532-43b7-4bb0-870a-6076125252d6"), "2e58a304-b0d6-4995-b235-1ec166c95721", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09771dd1-74de-45d8-aa96-34e5e4614822"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5d13c532-43b7-4bb0-870a-6076125252d6"));
        }
    }
}
