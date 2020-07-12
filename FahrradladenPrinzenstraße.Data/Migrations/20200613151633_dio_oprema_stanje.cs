using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class dio_oprema_stanje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaDio_Dio_DioId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaDio_DioStanje_DioStanjeId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaOprema_Oprema_OpremaId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaOprema_OpremaStanje_OpremaStanjeId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaProdajaOprema_OpremaId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaProdajaDio_DioId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.DropColumn(
                name: "OpremaId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.DropColumn(
                name: "DioId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.AlterColumn<int>(
                name: "OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DioStanjeId",
                table: "RezervacijaProdajaDio",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaDio_DioStanje_DioStanjeId",
                table: "RezervacijaProdajaDio",
                column: "DioStanjeId",
                principalTable: "DioStanje",
                principalColumn: "DioStanjeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaOprema_OpremaStanje_OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaStanjeId",
                principalTable: "OpremaStanje",
                principalColumn: "OpremaStanjeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaDio_DioStanje_DioStanjeId",
                table: "RezervacijaProdajaDio");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaProdajaOprema_OpremaStanje_OpremaStanjeId",
                table: "RezervacijaProdajaOprema");

            migrationBuilder.AlterColumn<int>(
                name: "OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "OpremaId",
                table: "RezervacijaProdajaOprema",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DioStanjeId",
                table: "RezervacijaProdajaDio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DioId",
                table: "RezervacijaProdajaDio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaOprema_OpremaId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaProdajaDio_DioId",
                table: "RezervacijaProdajaDio",
                column: "DioId");

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaDio_Dio_DioId",
                table: "RezervacijaProdajaDio",
                column: "DioId",
                principalTable: "Dio",
                principalColumn: "DioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaDio_DioStanje_DioStanjeId",
                table: "RezervacijaProdajaDio",
                column: "DioStanjeId",
                principalTable: "DioStanje",
                principalColumn: "DioStanjeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaOprema_Oprema_OpremaId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaId",
                principalTable: "Oprema",
                principalColumn: "OpremaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaProdajaOprema_OpremaStanje_OpremaStanjeId",
                table: "RezervacijaProdajaOprema",
                column: "OpremaStanjeId",
                principalTable: "OpremaStanje",
                principalColumn: "OpremaStanjeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
