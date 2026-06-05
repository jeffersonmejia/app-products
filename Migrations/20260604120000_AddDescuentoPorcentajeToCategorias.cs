using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudProductos.Migrations
{
    /// <inheritdoc />
    public partial class AddDescuentoPorcentajeToCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoPorcentaje",
                table: "Categorias",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescuentoPorcentaje",
                table: "Categorias");
        }
    }
}
