using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableContasLancamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Clientes_ClienteId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Vendas_VendaId",
                table: "Lancamentos");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Lancamentos",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Lancamentos",
                newName: "tipo");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Lancamentos",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Baixado",
                table: "Lancamentos",
                newName: "baixado");

            migrationBuilder.RenameColumn(
                name: "FormaPgto",
                table: "Lancamentos",
                newName: "forma_pgto");

            migrationBuilder.RenameColumn(
                name: "DataVencimento",
                table: "Lancamentos",
                newName: "data_vencimento");

            migrationBuilder.RenameColumn(
                name: "DataPagamento",
                table: "Lancamentos",
                newName: "data_pagamento");

            migrationBuilder.RenameColumn(
                name: "ClienteFornecedor",
                table: "Lancamentos",
                newName: "cliente_fornecedor");

            migrationBuilder.RenameColumn(
                name: "AnexoBlob",
                table: "Lancamentos",
                newName: "anexo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Lancamentos",
                newName: "idLancamentos");

            migrationBuilder.RenameColumn(
                name: "VendaId",
                table: "Lancamentos",
                newName: "vendas_id");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Lancamentos",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "Lancamentos",
                newName: "contas_id");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamentos_VendaId",
                table: "Lancamentos",
                newName: "IX_Lancamentos_vendas_id");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamentos_ClienteId",
                table: "Lancamentos",
                newName: "IX_Lancamentos_usuario_id");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamentos_CategoriaId",
                table: "Lancamentos",
                newName: "IX_Lancamentos_contas_id");

            migrationBuilder.AlterColumn<int>(
                name: "baixado",
                table: "Lancamentos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "categorias_id",
                table: "Lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clientes_id",
                table: "Lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "desconto",
                table: "Lancamentos",
                type: "decimal(15,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "observacoes",
                table: "Lancamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "tipo_desconto",
                table: "Lancamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "valor_desconto",
                table: "Lancamentos",
                type: "decimal(15,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    idContas = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    conta = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    banco = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    saldo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    tipo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.idContas);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$uckBe1VCbewl0jzxKkqYc.EKIBKPaHTNJ5f5muG67M5kRhfuxeBCy");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_categorias_id",
                table: "Lancamentos",
                column: "categorias_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_clientes_id",
                table: "Lancamentos",
                column: "clientes_id");

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

            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropIndex(
                name: "IX_Lancamentos_categorias_id",
                table: "Lancamentos");

            migrationBuilder.DropIndex(
                name: "IX_Lancamentos_clientes_id",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "categorias_id",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "clientes_id",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "desconto",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "observacoes",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "tipo_desconto",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "valor_desconto",
                table: "Lancamentos");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "Lancamentos",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "Lancamentos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "Lancamentos",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "baixado",
                table: "Lancamentos",
                newName: "Baixado");

            migrationBuilder.RenameColumn(
                name: "forma_pgto",
                table: "Lancamentos",
                newName: "FormaPgto");

            migrationBuilder.RenameColumn(
                name: "data_vencimento",
                table: "Lancamentos",
                newName: "DataVencimento");

            migrationBuilder.RenameColumn(
                name: "data_pagamento",
                table: "Lancamentos",
                newName: "DataPagamento");

            migrationBuilder.RenameColumn(
                name: "cliente_fornecedor",
                table: "Lancamentos",
                newName: "ClienteFornecedor");

            migrationBuilder.RenameColumn(
                name: "anexo",
                table: "Lancamentos",
                newName: "AnexoBlob");

            migrationBuilder.RenameColumn(
                name: "idLancamentos",
                table: "Lancamentos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendas_id",
                table: "Lancamentos",
                newName: "VendaId");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "Lancamentos",
                newName: "ClienteId");

            migrationBuilder.RenameColumn(
                name: "contas_id",
                table: "Lancamentos",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamentos_vendas_id",
                table: "Lancamentos",
                newName: "IX_Lancamentos_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamentos_usuario_id",
                table: "Lancamentos",
                newName: "IX_Lancamentos_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Lancamentos_contas_id",
                table: "Lancamentos",
                newName: "IX_Lancamentos_CategoriaId");

            migrationBuilder.AlterColumn<int>(
                name: "Baixado",
                table: "Lancamentos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$EqUikjHYKXlQLVhjrSWIsO/Mre9t69t3/o/r1./Sp6bH4PRItridO");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Clientes_ClienteId",
                table: "Lancamentos",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Vendas_VendaId",
                table: "Lancamentos",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
