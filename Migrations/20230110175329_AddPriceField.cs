using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resorty.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PriceTotal",
                table: "Reservation",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceTotal",
                table: "Reservation");
        }
    }
}
