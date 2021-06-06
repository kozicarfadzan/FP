using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class rezervacijaservis_kod_iszavrseno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsZavrseno",
                table: "RezervacijaServis",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Kod",
                table: "RezervacijaServis",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsZavrseno",
                table: "RezervacijaServis");

            migrationBuilder.DropColumn(
                name: "Kod",
                table: "RezervacijaServis");
        }
    }
}
