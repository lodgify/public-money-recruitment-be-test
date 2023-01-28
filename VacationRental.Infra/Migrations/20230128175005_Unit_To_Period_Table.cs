using Microsoft.EntityFrameworkCore.Migrations;

namespace VacationRental.Infra.Migrations
{
    public partial class Unit_To_Period_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Unity",
                table: "PreparationTimes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unity",
                table: "PreparationTimes");
        }
    }
}
