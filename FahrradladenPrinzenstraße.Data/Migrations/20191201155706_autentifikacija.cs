using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class autentifikacija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrator_Korisnik_KorisnikID",
                table: "Administrator");

            migrationBuilder.DropForeignKey(
                name: "FK_Zaposlenik_Korisnik_KorisnikID",
                table: "Zaposlenik");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zaposlenik",
                table: "Zaposlenik");

            migrationBuilder.DropIndex(
                name: "IX_Zaposlenik_KorisnikID",
                table: "Zaposlenik");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator");

            migrationBuilder.DropIndex(
                name: "IX_Administrator_KorisnikID",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "ZaposlenikID",
                table: "Zaposlenik");

            migrationBuilder.DropColumn(
                name: "AdministratorID",
                table: "Administrator");

            migrationBuilder.RenameColumn(
                name: "KorisnikID",
                table: "Zaposlenik",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "KorisnikID",
                table: "Administrator",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zaposlenik",
                table: "Zaposlenik",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AutorizacijskiToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Vrijednost = table.Column<string>(nullable: true),
                    KorisnikId = table.Column<int>(nullable: false),
                    VrijemeEvidentiranja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacijskiToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutorizacijskiToken_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacijskiToken_KorisnikId",
                table: "AutorizacijskiToken",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrator_Korisnik_Id",
                table: "Administrator",
                column: "Id",
                principalTable: "Korisnik",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zaposlenik_Korisnik_Id",
                table: "Zaposlenik",
                column: "Id",
                principalTable: "Korisnik",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrator_Korisnik_Id",
                table: "Administrator");

            migrationBuilder.DropForeignKey(
                name: "FK_Zaposlenik_Korisnik_Id",
                table: "Zaposlenik");

            migrationBuilder.DropTable(
                name: "AutorizacijskiToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zaposlenik",
                table: "Zaposlenik");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Zaposlenik",
                newName: "KorisnikID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Administrator",
                newName: "KorisnikID");

            migrationBuilder.AddColumn<int>(
                name: "ZaposlenikID",
                table: "Zaposlenik",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "AdministratorID",
                table: "Administrator",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zaposlenik",
                table: "Zaposlenik",
                column: "ZaposlenikID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposlenik_KorisnikID",
                table: "Zaposlenik",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_KorisnikID",
                table: "Administrator",
                column: "KorisnikID");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrator_Korisnik_KorisnikID",
                table: "Administrator",
                column: "KorisnikID",
                principalTable: "Korisnik",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zaposlenik_Korisnik_KorisnikID",
                table: "Zaposlenik",
                column: "KorisnikID",
                principalTable: "Korisnik",
                principalColumn: "KorisnikID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
