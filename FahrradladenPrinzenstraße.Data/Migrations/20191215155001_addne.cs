using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class addne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lokacija",
                table: "BiciklStanje");

            migrationBuilder.AddColumn<int>(
                name: "LokacijaId",
                table: "BiciklStanje",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lokacija",
                columns: table => new
                {
                    LokacijaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacija", x => x.LokacijaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiciklStanje_LokacijaId",
                table: "BiciklStanje",
                column: "LokacijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiciklStanje_Lokacija_LokacijaId",
                table: "BiciklStanje",
                column: "LokacijaId",
                principalTable: "Lokacija",
                principalColumn: "LokacijaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiciklStanje_Lokacija_LokacijaId",
                table: "BiciklStanje");

            migrationBuilder.DropTable(
                name: "Lokacija");

            migrationBuilder.DropIndex(
                name: "IX_BiciklStanje_LokacijaId",
                table: "BiciklStanje");

            migrationBuilder.DropColumn(
                name: "LokacijaId",
                table: "BiciklStanje");

            migrationBuilder.AddColumn<int>(
                name: "Lokacija",
                table: "BiciklStanje",
                nullable: false,
                defaultValue: 0);
        }
    }
}
