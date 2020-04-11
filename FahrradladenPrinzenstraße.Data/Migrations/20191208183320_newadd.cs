using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class newadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boja",
                columns: table => new
                {
                    BojaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boja", x => x.BojaId);
                });

            migrationBuilder.CreateTable(
                name: "MaterijalOkvira",
                columns: table => new
                {
                    MaterijalOkviraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterijalOkvira", x => x.MaterijalOkviraId);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodjac",
                columns: table => new
                {
                    ProizvodjacId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodjac", x => x.ProizvodjacId);
                });

            migrationBuilder.CreateTable(
                name: "StarosnaGrupa",
                columns: table => new
                {
                    StarosnaGrupaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarosnaGrupa", x => x.StarosnaGrupaId);
                });

            migrationBuilder.CreateTable(
                name: "VelicinaOkvira",
                columns: table => new
                {
                    VelicinaOkviraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VelicinaOkvira", x => x.VelicinaOkviraId);
                });

            migrationBuilder.CreateTable(
                name: "Modeli",
                columns: table => new
                {
                    ModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    ProizvodjacId = table.Column<int>(nullable: false),
                    Brzina = table.Column<int>(nullable: false),
                    Suspenzija = table.Column<int>(nullable: false),
                    SpolBicikl = table.Column<int>(nullable: false),
                    Tip = table.Column<int>(nullable: false),
                    MaterijalOkviraId = table.Column<int>(nullable: false),
                    StarosnaGrupaId = table.Column<int>(nullable: false),
                    VelicinaOkviraId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modeli", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                        column: x => x.MaterijalOkviraId,
                        principalTable: "MaterijalOkvira",
                        principalColumn: "MaterijalOkviraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modeli_Proizvodjac_ProizvodjacId",
                        column: x => x.ProizvodjacId,
                        principalTable: "Proizvodjac",
                        principalColumn: "ProizvodjacId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modeli_StarosnaGrupa_StarosnaGrupaId",
                        column: x => x.StarosnaGrupaId,
                        principalTable: "StarosnaGrupa",
                        principalColumn: "StarosnaGrupaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modeli_VelicinaOkvira_VelicinaOkviraId",
                        column: x => x.VelicinaOkviraId,
                        principalTable: "VelicinaOkvira",
                        principalColumn: "VelicinaOkviraId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bicikl",
                columns: table => new
                {
                    BiciklId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModelId = table.Column<int>(nullable: false),
                    GodinaProizvodnje = table.Column<short>(nullable: false),
                    Stanje = table.Column<int>(nullable: false),
                    Slika = table.Column<string>(nullable: true),
                    CijenaPoDanu = table.Column<double>(nullable: true),
                    Cijena = table.Column<double>(nullable: true),
                    BojaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bicikl", x => x.BiciklId);
                    table.ForeignKey(
                        name: "FK_Bicikl_Boja_BojaId",
                        column: x => x.BojaId,
                        principalTable: "Boja",
                        principalColumn: "BojaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bicikl_Modeli_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Modeli",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiciklStanje",
                columns: table => new
                {
                    BiciklStanjeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BiciklId = table.Column<int>(nullable: false),
                    Lokacija = table.Column<int>(nullable: false),
                    Sifra = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiciklStanje", x => x.BiciklStanjeId);
                    table.ForeignKey(
                        name: "FK_BiciklStanje_Bicikl_BiciklId",
                        column: x => x.BiciklId,
                        principalTable: "Bicikl",
                        principalColumn: "BiciklId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bicikl_BojaId",
                table: "Bicikl",
                column: "BojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Bicikl_ModelId",
                table: "Bicikl",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BiciklStanje_BiciklId",
                table: "BiciklStanje",
                column: "BiciklId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_MaterijalOkviraId",
                table: "Modeli",
                column: "MaterijalOkviraId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_ProizvodjacId",
                table: "Modeli",
                column: "ProizvodjacId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_StarosnaGrupaId",
                table: "Modeli",
                column: "StarosnaGrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_VelicinaOkviraId",
                table: "Modeli",
                column: "VelicinaOkviraId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiciklStanje");

            migrationBuilder.DropTable(
                name: "Bicikl");

            migrationBuilder.DropTable(
                name: "Boja");

            migrationBuilder.DropTable(
                name: "Modeli");

            migrationBuilder.DropTable(
                name: "MaterijalOkvira");

            migrationBuilder.DropTable(
                name: "Proizvodjac");

            migrationBuilder.DropTable(
                name: "StarosnaGrupa");

            migrationBuilder.DropTable(
                name: "VelicinaOkvira");
        }
    }
}
