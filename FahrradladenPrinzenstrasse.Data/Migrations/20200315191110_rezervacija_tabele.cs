using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class rezervacija_tabele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dio",
                columns: table => new
                {
                    DioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Cijena = table.Column<double>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    ProizvodjacID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dio", x => x.DioId);
                    table.ForeignKey(
                        name: "FK_Dio_Proizvodjac_ProizvodjacID",
                        column: x => x.ProizvodjacID,
                        principalTable: "Proizvodjac",
                        principalColumn: "ProizvodjacId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oprema",
                columns: table => new
                {
                    OpremaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Cijena = table.Column<double>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    ProizvodjacID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oprema", x => x.OpremaId);
                    table.ForeignKey(
                        name: "FK_Oprema_Proizvodjac_ProizvodjacID",
                        column: x => x.ProizvodjacID,
                        principalTable: "Proizvodjac",
                        principalColumn: "ProizvodjacId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaProdajaBicikla",
                columns: table => new
                {
                    RezervacijaProdajaBiciklaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RezervacijaId = table.Column<int>(nullable: false),
                    BiciklStanjeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaProdajaBicikla", x => x.RezervacijaProdajaBiciklaID);
                    table.ForeignKey(
                        name: "FK_RezervacijaProdajaBicikla_BiciklStanje_BiciklStanjeId",
                        column: x => x.BiciklStanjeId,
                        principalTable: "BiciklStanje",
                        principalColumn: "BiciklStanjeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaProdajaBicikla_Rezervacija_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servis",
                columns: table => new
                {
                    ServisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Cijena = table.Column<double>(nullable: false),
                    Opis = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servis", x => x.ServisId);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaProdajaDio",
                columns: table => new
                {
                    RezervacijaProdajaDioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RezervacijaId = table.Column<int>(nullable: false),
                    DioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaProdajaDio", x => x.RezervacijaProdajaDioId);
                    table.ForeignKey(
                        name: "FK_RezervacijaProdajaDio_Dio_DioId",
                        column: x => x.DioId,
                        principalTable: "Dio",
                        principalColumn: "DioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaProdajaDio_Rezervacija_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaProdajaOprema",
                columns: table => new
                {
                    RezervacijaProdajaOpremaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RezervacijaId = table.Column<int>(nullable: false),
                    OpremaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaProdajaOprema", x => x.RezervacijaProdajaOpremaId);
                    table.ForeignKey(
                        name: "FK_RezervacijaProdajaOprema_Oprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "Oprema",
                        principalColumn: "OpremaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaProdajaOprema_Rezervacija_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaServis",
                columns: table => new
                {
                    RezervacijaServisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RezervacijaId = table.Column<int>(nullable: false),
                    ServisId = table.Column<int>(nullable: false),
                    DatumServisiranja = table.Column<DateTime>(nullable: false),
                    IsOdobreno = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaServis", x => x.RezervacijaServisId);
                    table.ForeignKey(
                        name: "FK_RezervacijaServis_Rezervacija_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaServis_Servis_ServisId",
                        column: x => x.ServisId,
                        principalTable: "Servis",
                        principalColumn: "ServisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dio_ProizvodjacID",
                table: "Dio",
                column: "ProizvodjacID");

            migrationBuilder.CreateIndex(
                name: "IX_Oprema_ProizvodjacID",
                table: "Oprema",
                column: "ProizvodjacID");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaBicikla_BiciklStanjeId",
                table: "RezervacijaProdajaBicikla",
                column: "BiciklStanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaBicikla_RezervacijaId",
                table: "RezervacijaProdajaBicikla",
                column: "RezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaDio_DioId",
                table: "RezervacijaProdajaDio",
                column: "DioId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaDio_RezervacijaId",
                table: "RezervacijaProdajaDio",
                column: "RezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaOprema_OpremaId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaOprema_RezervacijaId",
                table: "RezervacijaProdajaOprema",
                column: "RezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaServis_RezervacijaId",
                table: "RezervacijaServis",
                column: "RezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaServis_ServisId",
                table: "RezervacijaServis",
                column: "ServisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RezervacijaProdajaBicikla");

            migrationBuilder.DropTable(
                name: "RezervacijaProdajaDio");

            migrationBuilder.DropTable(
                name: "RezervacijaProdajaOprema");

            migrationBuilder.DropTable(
                name: "RezervacijaServis");

            migrationBuilder.DropTable(
                name: "Dio");

            migrationBuilder.DropTable(
                name: "Oprema");

            migrationBuilder.DropTable(
                name: "Servis");
        }
    }
}
