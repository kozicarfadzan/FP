using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class Oprema_Dio_Stanje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DioStanjeId",
                table: "RezervacijaProdajaDio",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DioStanje",
                columns: table => new
                {
                    DioStanjeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DioId = table.Column<int>(nullable: false),
                    LokacijaId = table.Column<int>(nullable: false),
                    Sifra = table.Column<string>(nullable: true),
                    Aktivan = table.Column<bool>(nullable: false),
                    KupacId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DioStanje", x => x.DioStanjeId);
                    table.ForeignKey(
                        name: "FK_DioStanje_Dio_DioId",
                        column: x => x.DioId,
                        principalTable: "Dio",
                        principalColumn: "DioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DioStanje_Klijent_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DioStanje_Lokacija_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacija",
                        principalColumn: "LokacijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpremaStanje",
                columns: table => new
                {
                    OpremaStanjeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpremaId = table.Column<int>(nullable: false),
                    LokacijaId = table.Column<int>(nullable: false),
                    Sifra = table.Column<string>(nullable: true),
                    Aktivan = table.Column<bool>(nullable: false),
                    KupacId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpremaStanje", x => x.OpremaStanjeId);
                    table.ForeignKey(
                        name: "FK_OpremaStanje_Klijent_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpremaStanje_Lokacija_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacija",
                        principalColumn: "LokacijaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpremaStanje_Oprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "Oprema",
                        principalColumn: "OpremaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaOprema_OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaStanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaDio_DioStanjeId",
                table: "RezervacijaProdajaDio",
                column: "DioStanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_DioStanje_DioId",
                table: "DioStanje",
                column: "DioId");

            migrationBuilder.CreateIndex(
                name: "IX_DioStanje_KupacId",
                table: "DioStanje",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_DioStanje_LokacijaId",
                table: "DioStanje",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_OpremaStanje_KupacId",
                table: "OpremaStanje",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_OpremaStanje_LokacijaId",
                table: "OpremaStanje",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_OpremaStanje_OpremaId",
                table: "OpremaStanje",
                column: "OpremaId");

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaDio_DioStanje_DioStanjeId",
                table: "RezervacijaProdajaDio",
                column: "DioStanjeId",
                principalTable: "DioStanje",
                principalColumn: "DioStanjeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaOprema_OpremaStanje_OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaStanjeId",
                principalTable: "OpremaStanje",
                principalColumn: "OpremaStanjeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaDio_DioStanje_DioStanjeId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaOprema_OpremaStanje_OpremaStanjeId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropTable(
                name: "DioStanje");

            migrationBuilder.DropTable(
                name: "OpremaStanje");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaProdajaOprema_OpremaStanjeId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaProdajaDio_DioStanjeId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.DropColumn(
                name: "OpremaStanjeId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropColumn(
                name: "DioStanjeId",
                table: "RezervacijaProdajaDio");
        }
    }
}
