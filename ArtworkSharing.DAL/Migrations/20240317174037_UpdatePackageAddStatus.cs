using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtworkSharing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackageAddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Packages");
        }
    }
}
