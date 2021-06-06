using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class rezervacija_stanje_datum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumRezervacije",
                table: "Rezervacija",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumUplate",
                table: "Rezervacija",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StanjeRezervacije",
                table: "Rezervacija",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumUplate",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "StanjeRezervacije",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumRezervacije",
                table: "Rezervacija",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
