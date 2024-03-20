using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("77d22bf1-0d8b-4707-8c19-3520af5deb43"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd8d2ad8-386c-46b4-869d-fee2d5c66f22"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cddca8be-e19a-4e62-aeee-4c1e2cc1ff6b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f87c6834-e88a-488e-9c82-3ad3ae4297a0"));

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "ArtworkServices");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaypalOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Intent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaypalOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaypalAmounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    ItemTotalCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemTotalValue = table.Column<double>(type: "float", nullable: false),
                    PaypalOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaypalAmounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaypalAmounts_PaypalOrders_PaypalOrderId",
                        column: x => x.PaypalOrderId,
                        principalTable: "PaypalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaypalItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaypalOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaypalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaypalItems_PaypalOrders_PaypalOrderId",
                        column: x => x.PaypalOrderId,
                        principalTable: "PaypalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5ac82a9b-65bc-4699-8b11-6ef4989564ec"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("a1eacf99-4616-4e86-8d7a-b5599ed26f4d"), null, "Artist", "ARTIST" },
                    { new Guid("d265424e-05e1-482f-9694-c13e4ae802f2"), null, "Audience", "AUDIENCE" },
                    { new Guid("ffbd8961-3aed-4b2f-9121-2a6795f0f525"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaypalAmounts_PaypalOrderId",
                table: "PaypalAmounts",
                column: "PaypalOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaypalItems_PaypalOrderId",
                table: "PaypalItems",
                column: "PaypalOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaypalAmounts");

            migrationBuilder.DropTable(
                name: "PaypalItems");

            migrationBuilder.DropTable(
                name: "PaypalOrders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5ac82a9b-65bc-4699-8b11-6ef4989564ec"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a1eacf99-4616-4e86-8d7a-b5599ed26f4d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d265424e-05e1-482f-9694-c13e4ae802f2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ffbd8961-3aed-4b2f-9121-2a6795f0f525"));

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Artists");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "ArtworkServices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
