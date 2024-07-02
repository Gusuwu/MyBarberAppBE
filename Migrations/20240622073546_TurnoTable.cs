using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBarberAPI.Migrations
{
    public partial class TurnoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Turno",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBarbero = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<int>(nullable: false),
                    IdPrecio = table.Column<int>(nullable: false),
                    Dia = table.Column<DateTime>(nullable: false),
                    Horario = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turno_Usuario_IdBarbero",
                        column: x => x.IdBarbero,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Turno_Precio_IdPrecio",
                        column: x => x.IdPrecio,
                        principalTable: "Precio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turno_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turno_IdBarbero",
                table: "Turno",
                column: "IdBarbero");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_IdPrecio",
                table: "Turno",
                column: "IdPrecio");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_IdUsuario",
                table: "Turno",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Turno");
        }
    }
}
