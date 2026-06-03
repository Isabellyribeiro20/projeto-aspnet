using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patudinhos.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarFotoECarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Produtos");
        }
    }
}
