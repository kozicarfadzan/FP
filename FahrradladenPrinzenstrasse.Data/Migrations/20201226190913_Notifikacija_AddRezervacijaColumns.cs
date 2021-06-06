using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class Notifikacija_AddRezervacijaColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RezervacijaProdajaBiciklaID",
                table: "Notifikacija",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RezervacijaProdajaDioId",
                table: "Notifikacija",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RezervacijaProdajaOpremaId",
                table: "Notifikacija",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RezervacijaServisId",
                table: "Notifikacija",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_RezervacijaProdajaBiciklaID",
                table: "Notifikacija",
                column: "RezervacijaProdajaBiciklaID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_RezervacijaProdajaDioId",
                table: "Notifikacija",
                column: "RezervacijaProdajaDioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_RezervacijaProdajaOpremaId",
                table: "Notifikacija",
                column: "RezervacijaProdajaOpremaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_RezervacijaServisId",
                table: "Notifikacija",
                column: "RezervacijaServisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifikacija_RezervacijaProdajaBicikla_RezervacijaProdajaBiciklaID",
                table: "Notifikacija",
                column: "RezervacijaProdajaBiciklaID",
                principalTable: "RezervacijaProdajaBicikla",
                principalColumn: "RezervacijaProdajaBiciklaID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifikacija_RezervacijaProdajaDio_RezervacijaProdajaDioId",
                table: "Notifikacija",
                column: "RezervacijaProdajaDioId",
                principalTable: "RezervacijaProdajaDio",
                principalColumn: "RezervacijaProdajaDioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifikacija_RezervacijaProdajaOprema_RezervacijaProdajaOpremaId",
                table: "Notifikacija",
                column: "RezervacijaProdajaOpremaId",
                principalTable: "RezervacijaProdajaOprema",
                principalColumn: "RezervacijaProdajaOpremaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifikacija_RezervacijaServis_RezervacijaServisId",
                table: "Notifikacija",
                column: "RezervacijaServisId",
                principalTable: "RezervacijaServis",
                principalColumn: "RezervacijaServisId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifikacija_RezervacijaProdajaBicikla_RezervacijaProdajaBiciklaID",
                table: "Notifikacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifikacija_RezervacijaProdajaDio_RezervacijaProdajaDioId",
                table: "Notifikacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifikacija_RezervacijaProdajaOprema_RezervacijaProdajaOpremaId",
                table: "Notifikacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifikacija_RezervacijaServis_RezervacijaServisId",
                table: "Notifikacija");

            migrationBuilder.DropIndex(
                name: "IX_Notifikacija_RezervacijaProdajaBiciklaID",
                table: "Notifikacija");

            migrationBuilder.DropIndex(
                name: "IX_Notifikacija_RezervacijaProdajaDioId",
                table: "Notifikacija");

            migrationBuilder.DropIndex(
                name: "IX_Notifikacija_RezervacijaProdajaOpremaId",
                table: "Notifikacija");

            migrationBuilder.DropIndex(
                name: "IX_Notifikacija_RezervacijaServisId",
                table: "Notifikacija");

            migrationBuilder.DropColumn(
                name: "RezervacijaProdajaBiciklaID",
                table: "Notifikacija");

            migrationBuilder.DropColumn(
                name: "RezervacijaProdajaDioId",
                table: "Notifikacija");

            migrationBuilder.DropColumn(
                name: "RezervacijaProdajaOpremaId",
                table: "Notifikacija");

            migrationBuilder.DropColumn(
                name: "RezervacijaServisId",
                table: "Notifikacija");
        }
    }
}
