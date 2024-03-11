using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init_Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VNPayTransactionRefunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TmnCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxnRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VNPayTransactionRefunds", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VNPayTransactionRefunds");
        }
    }
}
