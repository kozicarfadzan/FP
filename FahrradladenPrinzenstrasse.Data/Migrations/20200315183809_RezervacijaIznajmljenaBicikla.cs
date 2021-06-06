using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class RezervacijaIznajmljenaBicikla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RezervacijaIznajmljenaBicikla",
                columns: table => new
                {
                    RezervacijaIznajmljenaBiciklaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RezervacijaId = table.Column<int>(nullable: false),
                    BiciklStanjeId = table.Column<int>(nullable: false),
                    DatumPreuzimanja = table.Column<DateTime>(nullable: false),
                    DatumVracanja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaIznajmljenaBicikla", x => x.RezervacijaIznajmljenaBiciklaID);
                    table.ForeignKey(
                        name: "FK_RezervacijaIznajmljenaBicikla_BiciklStanje_BiciklStanjeId",
                        column: x => x.BiciklStanjeId,
                        principalTable: "BiciklStanje",
                        principalColumn: "BiciklStanjeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaIznajmljenaBicikla_Rezervacija_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "RezervacijaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaIznajmljenaBicikla_BiciklStanjeId",
                table: "RezervacijaIznajmljenaBicikla",
                column: "BiciklStanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaIznajmljenaBicikla_RezervacijaId",
                table: "RezervacijaIznajmljenaBicikla",
                column: "RezervacijaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RezervacijaIznajmljenaBicikla");
        }
    }
}
