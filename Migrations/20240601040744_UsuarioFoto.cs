using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBarberAPI.Migrations
{
    public partial class UsuarioFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Usuario",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
            name: "Notas",
            table: "Usuario",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Usuario");
            migrationBuilder.AlterColumn<string>(
            name: "Notas",
            table: "Usuario",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
        }
    }
}
