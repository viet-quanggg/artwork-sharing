using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5b086f3f-d5db-47b8-b557-aca49e7e1eae"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6e8e260d-25f9-4bb1-ac62-cc96d659cb8d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("70116a71-d624-42dd-bd6e-994ce077e7b2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d5f5ded0-75b1-4cf3-be99-803525992df2"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("77d22bf1-0d8b-4707-8c19-3520af5deb43"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("bd8d2ad8-386c-46b4-869d-fee2d5c66f22"), null, "Artist", "ARTIST" },
                    { new Guid("cddca8be-e19a-4e62-aeee-4c1e2cc1ff6b"), null, "Audience", "AUDIENCE" },
                    { new Guid("f87c6834-e88a-488e-9c82-3ad3ae4297a0"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
