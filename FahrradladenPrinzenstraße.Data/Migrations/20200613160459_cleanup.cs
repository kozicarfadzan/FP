using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Rezervacija");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
