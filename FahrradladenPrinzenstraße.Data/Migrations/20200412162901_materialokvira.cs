using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class materialokvira : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                table: "Modeli");

            migrationBuilder.AlterColumn<int>(
                name: "MaterijalOkviraId",
                table: "Modeli",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                table: "Modeli",
                column: "MaterijalOkviraId",
                principalTable: "MaterijalOkvira",
                principalColumn: "MaterijalOkviraId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                table: "Modeli");

            migrationBuilder.AlterColumn<int>(
                name: "MaterijalOkviraId",
                table: "Modeli",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                table: "Modeli",
                column: "MaterijalOkviraId",
                principalTable: "MaterijalOkvira",
                principalColumn: "MaterijalOkviraId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
