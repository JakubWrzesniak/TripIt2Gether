using Microsoft.EntityFrameworkCore.Migrations;

namespace TripIt2Gether.Migrations
{
    public partial class AddtripCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Trips",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Trips");
        }
    }
}
