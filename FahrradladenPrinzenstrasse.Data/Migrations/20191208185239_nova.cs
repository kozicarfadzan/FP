﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FahrradladenPrinzenstrasse.Data.Migrations
{
    public partial class nova : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                table: "Modeli");

            migrationBuilder.DropTable(
                name: "MaterijalOkvira");

            migrationBuilder.DropIndex(
                name: "IX_Modeli_MaterijalOkviraId",
                table: "Modeli");

            migrationBuilder.DropColumn(
                name: "MaterijalOkviraId",
                table: "Modeli");

            migrationBuilder.AddColumn<int>(
                name: "MaterijalOkvira",
                table: "Modeli",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterijalOkvira",
                table: "Modeli");

            migrationBuilder.AddColumn<int>(
                name: "MaterijalOkviraId",
                table: "Modeli",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_MaterijalOkviraId",
                table: "Modeli",
                column: "MaterijalOkviraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modeli_MaterijalOkvira_MaterijalOkviraId",
                table: "Modeli",
                column: "MaterijalOkviraId",
                principalTable: "MaterijalOkvira",
                principalColumn: "MaterijalOkviraId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
