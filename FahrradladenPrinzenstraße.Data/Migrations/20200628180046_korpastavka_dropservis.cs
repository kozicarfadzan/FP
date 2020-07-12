using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class korpastavka_dropservis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KorpaStavka_Servis_ServisId",
                table: "KorpaStavka");

            migrationBuilder.DropIndex(
                name: "IX_KorpaStavka_ServisId",
                table: "KorpaStavka");

            migrationBuilder.DropColumn(
                name: "ServisId",
                table: "KorpaStavka");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServisId",
                table: "KorpaStavka",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_ServisId",
                table: "KorpaStavka",
                column: "ServisId");

            migrationBuilder.AddForeignKey(
                name: "FK_KorpaStavka_Servis_ServisId",
                table: "KorpaStavka",
                column: "ServisId",
                principalTable: "Servis",
                principalColumn: "ServisId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
