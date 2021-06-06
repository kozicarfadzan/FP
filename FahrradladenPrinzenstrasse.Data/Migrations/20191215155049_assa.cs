using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class assa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiciklStanje_Lokacija_LokacijaId",
                table: "BiciklStanje");

            migrationBuilder.AlterColumn<int>(
                name: "LokacijaId",
                table: "BiciklStanje",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BiciklStanje_Lokacija_LokacijaId",
                table: "BiciklStanje",
                column: "LokacijaId",
                principalTable: "Lokacija",
                principalColumn: "LokacijaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiciklStanje_Lokacija_LokacijaId",
                table: "BiciklStanje");

            migrationBuilder.AlterColumn<int>(
                name: "LokacijaId",
                table: "BiciklStanje",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BiciklStanje_Lokacija_LokacijaId",
                table: "BiciklStanje",
                column: "LokacijaId",
                principalTable: "Lokacija",
                principalColumn: "LokacijaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
