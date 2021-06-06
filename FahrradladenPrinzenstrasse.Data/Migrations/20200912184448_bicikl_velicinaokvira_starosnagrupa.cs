using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class bicikl_velicinaokvira_starosnagrupa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeli_StarosnaGrupa_StarosnaGrupaId",
                table: "Modeli");

            migrationBuilder.DropForeignKey(
                name: "FK_Modeli_VelicinaOkvira_VelicinaOkviraId",
                table: "Modeli");

            migrationBuilder.DropIndex(
                name: "IX_Modeli_StarosnaGrupaId",
                table: "Modeli");

            migrationBuilder.DropIndex(
                name: "IX_Modeli_VelicinaOkviraId",
                table: "Modeli");

            migrationBuilder.DropColumn(
                name: "StarosnaGrupaId",
                table: "Modeli");

            migrationBuilder.DropColumn(
                name: "VelicinaOkviraId",
                table: "Modeli");

            migrationBuilder.AddColumn<int>(
                name: "StarosnaGrupaId",
                table: "Bicikl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VelicinaOkviraId",
                table: "Bicikl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bicikl_StarosnaGrupaId",
                table: "Bicikl",
                column: "StarosnaGrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_Bicikl_VelicinaOkviraId",
                table: "Bicikl",
                column: "VelicinaOkviraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bicikl_StarosnaGrupa_StarosnaGrupaId",
                table: "Bicikl",
                column: "StarosnaGrupaId",
                principalTable: "StarosnaGrupa",
                principalColumn: "StarosnaGrupaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bicikl_VelicinaOkvira_VelicinaOkviraId",
                table: "Bicikl",
                column: "VelicinaOkviraId",
                principalTable: "VelicinaOkvira",
                principalColumn: "VelicinaOkviraId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicikl_StarosnaGrupa_StarosnaGrupaId",
                table: "Bicikl");

            migrationBuilder.DropForeignKey(
                name: "FK_Bicikl_VelicinaOkvira_VelicinaOkviraId",
                table: "Bicikl");

            migrationBuilder.DropIndex(
                name: "IX_Bicikl_StarosnaGrupaId",
                table: "Bicikl");

            migrationBuilder.DropIndex(
                name: "IX_Bicikl_VelicinaOkviraId",
                table: "Bicikl");

            migrationBuilder.DropColumn(
                name: "StarosnaGrupaId",
                table: "Bicikl");

            migrationBuilder.DropColumn(
                name: "VelicinaOkviraId",
                table: "Bicikl");

            migrationBuilder.AddColumn<int>(
                name: "StarosnaGrupaId",
                table: "Modeli",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VelicinaOkviraId",
                table: "Modeli",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_StarosnaGrupaId",
                table: "Modeli",
                column: "StarosnaGrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_VelicinaOkviraId",
                table: "Modeli",
                column: "VelicinaOkviraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modeli_StarosnaGrupa_StarosnaGrupaId",
                table: "Modeli",
                column: "StarosnaGrupaId",
                principalTable: "StarosnaGrupa",
                principalColumn: "StarosnaGrupaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modeli_VelicinaOkvira_VelicinaOkviraId",
                table: "Modeli",
                column: "VelicinaOkviraId",
                principalTable: "VelicinaOkvira",
                principalColumn: "VelicinaOkviraId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
