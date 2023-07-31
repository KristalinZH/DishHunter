using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DishHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForgottenPropertiesAndNewConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_RestaurantOwners_RestaurantOwnerId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantOwners_AspNetUsers_ApplicationUserId",
                table: "RestaurantOwners");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantOwners_ApplicationUserId",
                table: "RestaurantOwners");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RestaurantOwners");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RestaurantOwners",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RestaurantOwners",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantOwnerId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantOwnerId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RestaurantOwnerId",
                table: "AspNetUsers",
                column: "RestaurantOwnerId",
                unique: true,
                filter: "[RestaurantOwnerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RestaurantOwners_RestaurantOwnerId",
                table: "AspNetUsers",
                column: "RestaurantOwnerId",
                principalTable: "RestaurantOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_RestaurantOwners_RestaurantOwnerId",
                table: "Brands",
                column: "RestaurantOwnerId",
                principalTable: "RestaurantOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RestaurantOwners_RestaurantOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_RestaurantOwners_RestaurantOwnerId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RestaurantOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RestaurantOwners");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RestaurantOwners");

            migrationBuilder.DropColumn(
                name: "RestaurantOwnerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "RestaurantOwners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantOwnerId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOwners_ApplicationUserId",
                table: "RestaurantOwners",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_RestaurantOwners_RestaurantOwnerId",
                table: "Brands",
                column: "RestaurantOwnerId",
                principalTable: "RestaurantOwners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantOwners_AspNetUsers_ApplicationUserId",
                table: "RestaurantOwners",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
