using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableLancamentosAdjustFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Categorias_categorias_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Clientes_clientes_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Contas_contas_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Usuarios_usuario_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Vendas_vendas_id",
                table: "Lancamentos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$cPLl04pENZw4GKZAZcbPJuZM6KFBYSAa4dI9nx/TZUne3.zricFWO");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Categorias_categorias_id",
                table: "Lancamentos",
                column: "categorias_id",
                principalTable: "Categorias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Clientes_clientes_id",
                table: "Lancamentos",
                column: "clientes_id",
                principalTable: "Clientes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Contas_contas_id",
                table: "Lancamentos",
                column: "contas_id",
                principalTable: "Contas",
                principalColumn: "idContas");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Usuarios_usuario_id",
                table: "Lancamentos",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Vendas_vendas_id",
                table: "Lancamentos",
                column: "vendas_id",
                principalTable: "Vendas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Categorias_categorias_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Clientes_clientes_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Contas_contas_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Usuarios_usuario_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Vendas_vendas_id",
                table: "Lancamentos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$QwUVqIvTXqVHqPBuY0bYMeVTgyaJT0Ekc1GpnwFB4ZN3z58llE0ka");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Categorias_categorias_id",
                table: "Lancamentos",
                column: "categorias_id",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Clientes_clientes_id",
                table: "Lancamentos",
                column: "clientes_id",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Contas_contas_id",
                table: "Lancamentos",
                column: "contas_id",
                principalTable: "Contas",
                principalColumn: "idContas",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Usuarios_usuario_id",
                table: "Lancamentos",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Vendas_vendas_id",
                table: "Lancamentos",
                column: "vendas_id",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
