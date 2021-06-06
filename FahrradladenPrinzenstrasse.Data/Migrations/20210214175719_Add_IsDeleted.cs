using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class Add_IsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VelicinaOkvira",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TerminStavka",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StarosnaGrupa",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Servis",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rezervacija",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OcjenaProizvoda",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifikacija",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "NacinOtpreme",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Modeli",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MaterijalOkvira",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lokacija",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KorpaStavka",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Korisnik",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Grad",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Dobavljac",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Boja",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BiciklStanje",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "VelicinaOkvira");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TerminStavka");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StarosnaGrupa");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Servis");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OcjenaProizvoda");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notifikacija");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "NacinOtpreme");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Modeli");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MaterijalOkvira");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lokacija");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KorpaStavka");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Grad");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Dobavljac");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Boja");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BiciklStanje");
        }
    }
}
