using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoScale.Migrations
{
    /// <inheritdoc />
    public partial class fix_notis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usuario_id",
                table: "notificacao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_notificacao_usuario_id",
                table: "notificacao",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notificacao_usuario_usuario_id",
                table: "notificacao",
                column: "usuario_id",
                principalTable: "usuario",
                principalColumn: "usuario_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notificacao_usuario_usuario_id",
                table: "notificacao");

            migrationBuilder.DropIndex(
                name: "IX_notificacao_usuario_id",
                table: "notificacao");

            migrationBuilder.DropColumn(
                name: "usuario_id",
                table: "notificacao");
        }
    }
}
