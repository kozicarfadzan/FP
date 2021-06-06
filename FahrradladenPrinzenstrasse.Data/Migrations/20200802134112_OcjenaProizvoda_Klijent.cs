using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class OcjenaProizvoda_Klijent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KlijentId",
                table: "OcjenaProizvoda",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaProizvoda_KlijentId",
                table: "OcjenaProizvoda",
                column: "KlijentId");

            migrationBuilder.AddForeignKey(
                name: "FK_OcjenaProizvoda_Klijent_KlijentId",
                table: "OcjenaProizvoda",
                column: "KlijentId",
                principalTable: "Klijent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OcjenaProizvoda_Klijent_KlijentId",
                table: "OcjenaProizvoda");

            migrationBuilder.DropIndex(
                name: "IX_OcjenaProizvoda_KlijentId",
                table: "OcjenaProizvoda");

            migrationBuilder.DropColumn(
                name: "KlijentId",
                table: "OcjenaProizvoda");
        }
    }
}
