using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBarberAPI.Migrations
{
    public partial class columnNotDisponiblePrecio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotDisponible",
                table: "Precio",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotDisponible",
                table: "Precio");
        }
    }
}
