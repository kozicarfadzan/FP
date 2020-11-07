using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class bicikl_velicinaokvira_starosnagrupa_required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicikl_StarosnaGrupa_StarosnaGrupaId",
                table: "Bicikl");

            migrationBuilder.DropForeignKey(
                name: "FK_Bicikl_VelicinaOkvira_VelicinaOkviraId",
                table: "Bicikl");

            migrationBuilder.AlterColumn<int>(
                name: "VelicinaOkviraId",
                table: "Bicikl",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StarosnaGrupaId",
                table: "Bicikl",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bicikl_StarosnaGrupa_StarosnaGrupaId",
                table: "Bicikl",
                column: "StarosnaGrupaId",
                principalTable: "StarosnaGrupa",
                principalColumn: "StarosnaGrupaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bicikl_VelicinaOkvira_VelicinaOkviraId",
                table: "Bicikl",
                column: "VelicinaOkviraId",
                principalTable: "VelicinaOkvira",
                principalColumn: "VelicinaOkviraId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bicikl_StarosnaGrupa_StarosnaGrupaId",
                table: "Bicikl");

            migrationBuilder.DropForeignKey(
                name: "FK_Bicikl_VelicinaOkvira_VelicinaOkviraId",
                table: "Bicikl");

            migrationBuilder.AlterColumn<int>(
                name: "VelicinaOkviraId",
                table: "Bicikl",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "StarosnaGrupaId",
                table: "Bicikl",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

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
    }
}
