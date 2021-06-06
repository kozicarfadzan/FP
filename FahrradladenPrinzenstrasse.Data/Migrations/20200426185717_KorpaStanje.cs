using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class KorpaStanje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KorpaStavka",
                columns: table => new
                {
                    KorpaStavkaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlijentId = table.Column<int>(nullable: false),
                    BiciklId = table.Column<int>(nullable: true),
                    OpremaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorpaStavka", x => x.KorpaStavkaId);
                    table.ForeignKey(
                        name: "FK_KorpaStavka_Bicikl_BiciklId",
                        column: x => x.BiciklId,
                        principalTable: "Bicikl",
                        principalColumn: "BiciklId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KorpaStavka_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorpaStavka_Oprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "Oprema",
                        principalColumn: "OpremaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_BiciklId",
                table: "KorpaStavka",
                column: "BiciklId");

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_KlijentId",
                table: "KorpaStavka",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_OpremaId",
                table: "KorpaStavka",
                column: "OpremaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KorpaStavka");
        }
    }
}
