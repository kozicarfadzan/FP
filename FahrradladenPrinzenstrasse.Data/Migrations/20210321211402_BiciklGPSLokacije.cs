using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class BiciklGPSLokacije : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BiciklGPSLokacije",
                columns: table => new
                {
                    BiciklGPSLokacijeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiciklId = table.Column<int>(nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10, 6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10, 6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiciklGPSLokacije", x => x.BiciklGPSLokacijeId);
                    table.ForeignKey(
                        name: "FK_BiciklGPSLokacije_Bicikl_BiciklId",
                        column: x => x.BiciklId,
                        principalTable: "Bicikl",
                        principalColumn: "BiciklId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiciklGPSLokacije_BiciklId",
                table: "BiciklGPSLokacije",
                column: "BiciklId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiciklGPSLokacije");
        }
    }
}
