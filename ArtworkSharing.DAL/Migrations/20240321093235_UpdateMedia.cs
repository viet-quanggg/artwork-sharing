using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5e36c59b-f699-40de-a4d0-35567352dccd"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("87e4dc6b-95f3-4ed1-bf4b-a6b44910ed28"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c4e5e30a-7a9a-45da-b017-2f2c34f08f55"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ddadaefc-c3c6-4a66-b0b1-61ac53961f5e"));

            migrationBuilder.AddColumn<string>(
                name: "MediaWithoutWatermark",
                table: "MediaContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("00e2917b-9c8e-4cba-b39b-09bd9cebd30c"), null, "Audience", "AUDIENCE" },
                    { new Guid("20d40e62-c6c6-408a-9b27-71c7aaaea982"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("6193ea9f-91a9-41e2-aabc-27a35d93b612"), null, "Admin", "ADMIN" },
                    { new Guid("820fa473-2833-4dd6-9d04-eb8019bfbad5"), null, "Artist", "ARTIST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00e2917b-9c8e-4cba-b39b-09bd9cebd30c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("20d40e62-c6c6-408a-9b27-71c7aaaea982"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6193ea9f-91a9-41e2-aabc-27a35d93b612"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("820fa473-2833-4dd6-9d04-eb8019bfbad5"));

            migrationBuilder.DropColumn(
                name: "MediaWithoutWatermark",
                table: "MediaContents");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5e36c59b-f699-40de-a4d0-35567352dccd"), null, "Audience", "AUDIENCE" },
                    { new Guid("87e4dc6b-95f3-4ed1-bf4b-a6b44910ed28"), null, "Artist", "ARTIST" },
                    { new Guid("c4e5e30a-7a9a-45da-b017-2f2c34f08f55"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("ddadaefc-c3c6-4a66-b0b1-61ac53961f5e"), null, "Admin", "ADMIN" }
                });
        }
    }
}
