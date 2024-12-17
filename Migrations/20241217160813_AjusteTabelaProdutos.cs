using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTabelaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Produtos_Categorias_CategoriaId",
            //    table: "Produtos");

            //migrationBuilder.DropIndex(
            //    name: "IX_Produtos_CategoriaId",
            //    table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Produtos",
                newName: "PrecoVenda");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Produtos",
                newName: "TipoMovimento");

            migrationBuilder.RenameColumn(
                name: "Peso",
                table: "Produtos",
                newName: "MargemLucro");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "Produtos",
                newName: "CodigoBarras");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "Produtos",
                newName: "EstoqueMin");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "Produtos",
                newName: "Estoque");

            //migrationBuilder.AddColumn<int>(
            //    name: "CategoriaModelId",
            //    table: "Produtos",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoCompra",
                table: "Produtos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            //migrationBuilder.UpdateData(
            //    table: "Usuarios",
            //    keyColumn: "Id",
            //    keyValue: 1,
            //    column: "Senha",
            //    value: "$2a$11$eK1KdvyDrf2x3a4R97pkX.cBeAXOXzY18k.vZFCXcxx/c0JuzGF26");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Produtos_CategoriaModelId",
            //    table: "Produtos",
            //    column: "CategoriaModelId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Produtos_Categorias_CategoriaModelId",
            //    table: "Produtos",
            //    column: "CategoriaModelId",
            //    principalTable: "Categorias",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Produtos_Categorias_CategoriaModelId",
            //    table: "Produtos");

            //migrationBuilder.DropIndex(
            //    name: "IX_Produtos_CategoriaModelId",
            //    table: "Produtos");

            //migrationBuilder.DropColumn(
            //    name: "CategoriaModelId",
            //    table: "Produtos");

            migrationBuilder.DropColumn(
                name: "PrecoCompra",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "TipoMovimento",
                table: "Produtos",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PrecoVenda",
                table: "Produtos",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "MargemLucro",
                table: "Produtos",
                newName: "Peso");

            migrationBuilder.RenameColumn(
                name: "EstoqueMin",
                table: "Produtos",
                newName: "Codigo");

            migrationBuilder.RenameColumn(
                name: "Estoque",
                table: "Produtos",
                newName: "CategoriaId");

            migrationBuilder.RenameColumn(
                name: "CodigoBarras",
                table: "Produtos",
                newName: "Observacao");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$ce.X2fy4GYdVy.5UZeUTOOOqkpMDaLUAbHyh0ndqnNiXIlqOZqOU2");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Produtos_CategoriaId",
            //    table: "Produtos",
            //    column: "CategoriaId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Produtos_Categorias_CategoriaId",
            //    table: "Produtos",
            //    column: "CategoriaId",
            //    principalTable: "Categorias",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
