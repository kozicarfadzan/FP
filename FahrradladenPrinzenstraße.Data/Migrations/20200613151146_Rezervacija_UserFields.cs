using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class Rezervacija_UserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdresaStanovanja",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrojTelefona",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Država",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grad",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NacinOtpremeId",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NacinPlacanja",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Osnovica",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Pokrajina",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostanskiKod",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Postarina",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Prezime",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UkupniIznos",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UkupnoPoreza",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UkupnoProizvodi",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_NacinOtpremeId",
                table: "Rezervacija",
                column: "NacinOtpremeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_NacinOtpreme_NacinOtpremeId",
                table: "Rezervacija",
                column: "NacinOtpremeId",
                principalTable: "NacinOtpreme",
                principalColumn: "NacinOtpremeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_NacinOtpreme_NacinOtpremeId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_NacinOtpremeId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "AdresaStanovanja",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "BrojTelefona",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Država",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Grad",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "NacinOtpremeId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "NacinPlacanja",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Osnovica",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Pokrajina",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "PostanskiKod",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Postarina",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Prezime",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "UkupniIznos",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "UkupnoPoreza",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "UkupnoProizvodi",
                table: "Rezervacija");
        }
    }
}
