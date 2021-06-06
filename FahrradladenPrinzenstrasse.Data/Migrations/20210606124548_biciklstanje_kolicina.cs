using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class biciklstanje_kolicina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiciklStanje_Klijent_KupacId",
                table: "BiciklStanje");

            migrationBuilder.DropIndex(
                name: "IX_BiciklStanje_KupacId",
                table: "BiciklStanje");

            migrationBuilder.DropColumn(
                name: "KupacId",
                table: "BiciklStanje");

            migrationBuilder.DropColumn(
                name: "Sifra",
                table: "BiciklStanje");

            migrationBuilder.AddColumn<int>(
                name: "Kolicina",
                table: "BiciklStanje",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kolicina",
                table: "BiciklStanje");

            migrationBuilder.AddColumn<int>(
                name: "KupacId",
                table: "BiciklStanje",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sifra",
                table: "BiciklStanje",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BiciklStanje_KupacId",
                table: "BiciklStanje",
                column: "KupacId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiciklStanje_Klijent_KupacId",
                table: "BiciklStanje",
                column: "KupacId",
                principalTable: "Klijent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
