using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class Bicikl_BiciklStanje_Aktivan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktivan",
                table: "BiciklStanje",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Aktivan",
                table: "Bicikl",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktivan",
                table: "BiciklStanje");

            migrationBuilder.DropColumn(
                name: "Aktivan",
                table: "Bicikl");
        }
    }
}
