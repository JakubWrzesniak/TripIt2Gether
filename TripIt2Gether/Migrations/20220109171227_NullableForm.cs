using Microsoft.EntityFrameworkCore.Migrations;

namespace TripIt2Gether.Migrations
{
    public partial class NullableForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Forms_FormId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_FormId",
                table: "Trips");

            migrationBuilder.AlterColumn<string>(
                name: "TripId",
                table: "Forms",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_TripId",
                table: "Forms",
                column: "TripId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Trips_TripId",
                table: "Forms",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Trips_TripId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_TripId",
                table: "Forms");

            migrationBuilder.AlterColumn<string>(
                name: "TripId",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Trips_FormId",
                table: "Trips",
                column: "FormId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Forms_FormId",
                table: "Trips",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "FormId");
        }
    }
}
