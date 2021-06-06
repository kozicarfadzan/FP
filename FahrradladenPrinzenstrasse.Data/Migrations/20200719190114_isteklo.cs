using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class isteklo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isteklo",
                table: "RezervacijaIznajmljenaBicikla",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Notifikacija",
                columns: table => new
                {
                    NotifikacijaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZaposlenikId = table.Column<int>(nullable: false),
                    RezervacijaIznajmljenaBiciklaId = table.Column<int>(nullable: true),
                    Tip = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifikacija", x => x.NotifikacijaId);
                    table.ForeignKey(
                        name: "FK_Notifikacija_RezervacijaIznajmljenaBicikla_RezervacijaIznajmljenaBiciklaId",
                        column: x => x.RezervacijaIznajmljenaBiciklaId,
                        principalTable: "RezervacijaIznajmljenaBicikla",
                        principalColumn: "RezervacijaIznajmljenaBiciklaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifikacija_Zaposlenik_ZaposlenikId",
                        column: x => x.ZaposlenikId,
                        principalTable: "Zaposlenik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_RezervacijaIznajmljenaBiciklaId",
                table: "Notifikacija",
                column: "RezervacijaIznajmljenaBiciklaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifikacija_ZaposlenikId",
                table: "Notifikacija",
                column: "ZaposlenikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifikacija");

            migrationBuilder.DropColumn(
                name: "Isteklo",
                table: "RezervacijaIznajmljenaBicikla");
        }
    }
}
