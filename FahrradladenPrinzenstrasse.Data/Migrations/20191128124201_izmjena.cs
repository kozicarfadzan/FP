using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class izmjena : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnik_Spol_SpolID",
                table: "Korisnik");

            migrationBuilder.DropTable(
                name: "Spol");

            migrationBuilder.DropIndex(
                name: "IX_Korisnik_SpolID",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "SpolID",
                table: "Korisnik");

            migrationBuilder.AddColumn<string>(
                name: "Spol",
                table: "Korisnik",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Spol",
                table: "Korisnik");

            migrationBuilder.AddColumn<int>(
                name: "SpolID",
                table: "Korisnik",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Spol",
                columns: table => new
                {
                    SpolID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spol", x => x.SpolID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_SpolID",
                table: "Korisnik",
                column: "SpolID");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnik_Spol_SpolID",
                table: "Korisnik",
                column: "SpolID",
                principalTable: "Spol",
                principalColumn: "SpolID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
