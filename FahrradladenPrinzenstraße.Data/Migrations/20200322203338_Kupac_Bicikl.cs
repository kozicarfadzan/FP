using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class Kupac_Bicikl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KupacId",
                table: "BiciklStanje",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
