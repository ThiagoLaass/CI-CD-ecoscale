using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoScale.Migrations
{
    /// <inheritdoc />
    public partial class fix_empresa_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_relatorio_empresa_empresa_cnpj",
                table: "relatorio");

            migrationBuilder.RenameColumn(
                name: "empresa_cnpj",
                table: "relatorio",
                newName: "empresa_id");

            migrationBuilder.RenameIndex(
                name: "IX_relatorio_empresa_cnpj",
                table: "relatorio",
                newName: "IX_relatorio_empresa_id");

            migrationBuilder.AddForeignKey(
                name: "FK_relatorio_empresa_empresa_id",
                table: "relatorio",
                column: "empresa_id",
                principalTable: "empresa",
                principalColumn: "usuario_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_relatorio_empresa_empresa_id",
                table: "relatorio");

            migrationBuilder.RenameColumn(
                name: "empresa_id",
                table: "relatorio",
                newName: "empresa_cnpj");

            migrationBuilder.RenameIndex(
                name: "IX_relatorio_empresa_id",
                table: "relatorio",
                newName: "IX_relatorio_empresa_cnpj");

            migrationBuilder.AddForeignKey(
                name: "FK_relatorio_empresa_empresa_cnpj",
                table: "relatorio",
                column: "empresa_cnpj",
                principalTable: "empresa",
                principalColumn: "usuario_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
