using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TripIt2Gether.Migrations
{
    public partial class ApplicationRequiredFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantPreviewuserId",
                table: "Applications",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationPreviewDataViewModel",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    TripNumber = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationPreviewDataViewModel", x => x.userId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ParticipantPreviewuserId",
                table: "Applications",
                column: "ParticipantPreviewuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationPreviewDataViewModel_ParticipantPreviewuserId",
                table: "Applications",
                column: "ParticipantPreviewuserId",
                principalTable: "ApplicationPreviewDataViewModel",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationPreviewDataViewModel_ParticipantPreviewuserId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "ApplicationPreviewDataViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ParticipantPreviewuserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ParticipantPreviewuserId",
                table: "Applications");
        }
    }
}
