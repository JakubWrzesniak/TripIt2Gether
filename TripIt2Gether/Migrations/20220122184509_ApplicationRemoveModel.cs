using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TripIt2Gether.Migrations
{
    public partial class ApplicationRemoveModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantPreviewuserId",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationPreviewDataViewModel",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TripNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripStartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
    }
}
