using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "PlatformFee",
                table: "PaypalRefunds");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2d592ad5-c1e3-414b-8179-61db656731db"), null, "Audience", "AUDIENCE" },
                    { new Guid("cbe32d2a-516c-4841-930c-7c579f710683"), null, "Admin", "ADMIN" },
                    { new Guid("d710dd46-5cb3-44ef-9918-45f06a947d2b"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("ecf0be8b-c8ed-4caa-8cbd-673678876a21"), null, "Artist", "ARTIST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2d592ad5-c1e3-414b-8179-61db656731db"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cbe32d2a-516c-4841-930c-7c579f710683"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d710dd46-5cb3-44ef-9918-45f06a947d2b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ecf0be8b-c8ed-4caa-8cbd-673678876a21"));

            migrationBuilder.AddColumn<double>(
                name: "PlatformFee",
                table: "PaypalRefunds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
    }
}
