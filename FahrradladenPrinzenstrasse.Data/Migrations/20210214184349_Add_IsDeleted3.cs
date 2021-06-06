using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class Add_IsDeleted3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TerminStavka");

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
                table: "KorpaStavka");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Oprema",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Dio",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bicikl",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Oprema");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Dio");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bicikl");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TerminStavka",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rezervacija",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OcjenaProizvoda",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifikacija",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KorpaStavka",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
