using Microsoft.EntityFrameworkCore.Migrations;

namespace TripIt2Gether.Migrations
{
    public partial class ChangeTEXTSizeRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AdditionInformation",
                table: "Applications",
                maxLength: 455,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(455)",
                oldMaxLength: 455,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AdditionInformation",
                table: "Applications",
                type: "nvarchar(455)",
                maxLength: 455,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 455);
        }
    }
}
