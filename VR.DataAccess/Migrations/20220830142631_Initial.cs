using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VR.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblRental",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Units = table.Column<int>(type: "int", unicode: false, nullable: false),
                    PreparationTimeInDays = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRental_RentalId", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblBooking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalId = table.Column<int>(type: "int", unicode: false, nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nights = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBooking_BookingId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAthlete_tblTeam",
                        column: x => x.RentalId,
                        principalTable: "tblRental",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblBooking_RentalId",
                table: "tblBooking",
                column: "RentalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBooking");

            migrationBuilder.DropTable(
                name: "tblRental");
        }
    }
}
