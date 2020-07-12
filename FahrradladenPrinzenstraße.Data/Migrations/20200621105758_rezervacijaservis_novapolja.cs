using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class rezervacijaservis_novapolja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kod",
                table: "RezervacijaServis");

            migrationBuilder.AddColumn<string>(
                name: "Boja",
                table: "RezervacijaServis",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DodatniTroskovi",
                table: "RezervacijaServis",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "RezervacijaServis",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Opis",
                table: "RezervacijaServis",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proizvodjac",
                table: "RezervacijaServis",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tip",
                table: "RezervacijaServis",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Boja",
                table: "RezervacijaServis");

            migrationBuilder.DropColumn(
                name: "DodatniTroskovi",
                table: "RezervacijaServis");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "RezervacijaServis");

            migrationBuilder.DropColumn(
                name: "Opis",
                table: "RezervacijaServis");

            migrationBuilder.DropColumn(
                name: "Proizvodjac",
                table: "RezervacijaServis");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "RezervacijaServis");

            migrationBuilder.AddColumn<int>(
                name: "Kod",
                table: "RezervacijaServis",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
