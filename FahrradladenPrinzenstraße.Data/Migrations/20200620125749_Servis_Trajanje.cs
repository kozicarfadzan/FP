using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    public partial class Servis_Trajanje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Trajanje",
                table: "Servis",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trajanje",
                table: "Servis");
        }
    }
}
