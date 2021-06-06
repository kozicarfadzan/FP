using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class korpastavke_dio_kolicina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DioId",
                table: "KorpaStavka",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kolicina",
                table: "KorpaStavka",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_DioId",
                table: "KorpaStavka",
                column: "DioId");

            migrationBuilder.AddForeignKey(
                name: "FK_KorpaStavka_Dio_DioId",
                table: "KorpaStavka",
                column: "DioId",
                principalTable: "Dio",
                principalColumn: "DioId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KorpaStavka_Dio_DioId",
                table: "KorpaStavka");

            migrationBuilder.DropIndex(
                name: "IX_KorpaStavka_DioId",
                table: "KorpaStavka");

            migrationBuilder.DropColumn(
                name: "DioId",
                table: "KorpaStavka");

            migrationBuilder.DropColumn(
                name: "Kolicina",
                table: "KorpaStavka");
        }
    }
}
