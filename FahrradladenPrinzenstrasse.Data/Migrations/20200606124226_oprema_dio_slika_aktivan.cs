using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class oprema_dio_slika_aktivan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktivan",
                table: "Oprema",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Slika",
                table: "Oprema",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Aktivan",
                table: "Dio",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Slika",
                table: "Dio",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktivan",
                table: "Oprema");

            migrationBuilder.DropColumn(
                name: "Slika",
                table: "Oprema");

            migrationBuilder.DropColumn(
                name: "Aktivan",
                table: "Dio");

            migrationBuilder.DropColumn(
                name: "Slika",
                table: "Dio");
        }
    }
}
