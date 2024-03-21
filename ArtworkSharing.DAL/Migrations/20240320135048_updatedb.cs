using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("68110d0f-9f5c-4fda-b702-a0e3a4011a89"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("690ad74b-50ef-4ed2-bda3-f89650d873fe"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("96907eca-2dd1-4d66-b33e-721948481c4c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e8eb4b4e-1bac-4470-aee8-008d3534b0da"));

            migrationBuilder.AlterColumn<string>(
                name: "CaptureId",
                table: "PaypalOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3567e103-a4d0-47f0-a7f4-67c626a3f965"), null, "Audience", "AUDIENCE" },
                    { new Guid("6375a272-f7a6-42b2-ab3b-ea4107e6cd9a"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("8ff9e61b-4b4d-448d-94aa-690f96884e01"), null, "Artist", "ARTIST" },
                    { new Guid("f6965338-ed93-4642-84cd-c768074372c0"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3567e103-a4d0-47f0-a7f4-67c626a3f965"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6375a272-f7a6-42b2-ab3b-ea4107e6cd9a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8ff9e61b-4b4d-448d-94aa-690f96884e01"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f6965338-ed93-4642-84cd-c768074372c0"));

            migrationBuilder.AlterColumn<string>(
                name: "CaptureId",
                table: "PaypalOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("68110d0f-9f5c-4fda-b702-a0e3a4011a89"), null, "Audience", "AUDIENCE" },
                    { new Guid("690ad74b-50ef-4ed2-bda3-f89650d873fe"), null, "Admin", "ADMIN" },
                    { new Guid("96907eca-2dd1-4d66-b33e-721948481c4c"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("e8eb4b4e-1bac-4470-aee8-008d3534b0da"), null, "Artist", "ARTIST" }
                });
        }
    }
}
