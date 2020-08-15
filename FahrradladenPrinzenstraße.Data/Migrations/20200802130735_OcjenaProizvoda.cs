using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class OcjenaProizvoda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OcjenaProizvoda",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ocjena = table.Column<int>(nullable: false),
                    BiciklId = table.Column<int>(nullable: true),
                    DioId = table.Column<int>(nullable: true),
                    OpremaId = table.Column<int>(nullable: true),
                    DatumOcjene = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcjenaProizvoda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OcjenaProizvoda_Bicikl_BiciklId",
                        column: x => x.BiciklId,
                        principalTable: "Bicikl",
                        principalColumn: "BiciklId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OcjenaProizvoda_Dio_DioId",
                        column: x => x.DioId,
                        principalTable: "Dio",
                        principalColumn: "DioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OcjenaProizvoda_Oprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "Oprema",
                        principalColumn: "OpremaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaProizvoda_BiciklId",
                table: "OcjenaProizvoda",
                column: "BiciklId");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaProizvoda_DioId",
                table: "OcjenaProizvoda",
                column: "DioId");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaProizvoda_OpremaId",
                table: "OcjenaProizvoda",
                column: "OpremaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OcjenaProizvoda");
        }
    }
}
