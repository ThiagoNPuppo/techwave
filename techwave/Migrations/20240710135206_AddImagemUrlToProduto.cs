using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace techwave.Migrations
{
    public partial class AddImagemUrlToProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_UsuarioId",
                table: "Funcionario",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_Usuarios_UsuarioId",
                table: "Funcionario",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_Usuarios_UsuarioId",
                table: "Funcionario");

            migrationBuilder.DropIndex(
                name: "IX_Funcionario_UsuarioId",
                table: "Funcionario");

            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "Produto");
        }
    }
}
