using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class terminstavka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TerminStavka",
                columns: table => new
                {
                    TerminStavkaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlijentId = table.Column<int>(nullable: false),
                    BiciklId = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    DatumPreuzimanja = table.Column<DateTime>(nullable: false),
                    DatumVracanja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminStavka", x => x.TerminStavkaId);
                    table.ForeignKey(
                        name: "FK_TerminStavka_Bicikl_BiciklId",
                        column: x => x.BiciklId,
                        principalTable: "Bicikl",
                        principalColumn: "BiciklId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminStavka_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TerminStavka_BiciklId",
                table: "TerminStavka",
                column: "BiciklId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminStavka_KlijentId",
                table: "TerminStavka",
                column: "KlijentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TerminStavka");
        }
    }
}
