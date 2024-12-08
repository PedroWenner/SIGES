using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Categorias",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CaminhoImagem = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categorias", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Clientes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //        Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Sexo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        PessoaFisica = table.Column<int>(type: "int", nullable: false),
            //        Contato = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
            //        Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        Celular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //        Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Fornecedor = table.Column<int>(type: "int", nullable: false),
            //        Cep = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        Rua = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
            //        NumeroRua = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
            //        Complemento = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
            //        Bairro = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
            //        Cidade = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
            //        Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        UsuarioCadastro = table.Column<int>(type: "int", nullable: false),
            //        DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Clientes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Permissoes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Nome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
            //        Permissoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Situacao = table.Column<int>(type: "int", nullable: true),
            //        Data = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        UsuarioCadastro = table.Column<int>(type: "int", nullable: false),
            //        DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Permissoes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Lancamentos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TipoMovimento = table.Column<int>(type: "int", nullable: false),
            //        Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CategoriaId = table.Column<int>(type: "int", nullable: false),
            //        Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Lancamentos", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Lancamentos_Categorias_CategoriaId",
            //            column: x => x.CategoriaId,
            //            principalTable: "Categorias",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Produtos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        Codigo = table.Column<int>(type: "int", nullable: true),
            //        Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        UnidadeMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CategoriaId = table.Column<int>(type: "int", nullable: false),
            //        Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CaminhoImagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UsuarioCadastro = table.Column<int>(type: "int", nullable: false),
            //        DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Produtos", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Produtos_Categorias_CategoriaId",
            //            column: x => x.CategoriaId,
            //            principalTable: "Categorias",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Usuarios",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        RgNumero = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
            //        Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //        Cep = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        Rua = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
            //        NumeroRua = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
            //        Bairro = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
            //        Cidade = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
            //        Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
            //        DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        Situacao = table.Column<int>(type: "int", nullable: true),
            //        CaminhoImagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PermissaoId = table.Column<int>(type: "int", nullable: false),
            //        UsuarioCadastro = table.Column<int>(type: "int", nullable: false),
            //        DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Usuarios", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Usuarios_Permissoes_PermissaoId",
            //            column: x => x.PermissaoId,
            //            principalTable: "Permissoes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Pedidos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        ProdutoId = table.Column<int>(type: "int", nullable: false),
            //        Qtd = table.Column<int>(type: "int", nullable: false),
            //        Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        ClienteId = table.Column<int>(type: "int", nullable: false),
            //        DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Pedidos", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Pedidos_Clientes_ClienteId",
            //            column: x => x.ClienteId,
            //            principalTable: "Clientes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Pedidos_Produtos_ProdutoId",
            //            column: x => x.ProdutoId,
            //            principalTable: "Produtos",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lancamentos_CategoriaId",
            //    table: "Lancamentos",
            //    column: "CategoriaId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Pedidos_ClienteId",
            //    table: "Pedidos",
            //    column: "ClienteId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Pedidos_ProdutoId",
            //    table: "Pedidos",
            //    column: "ProdutoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Produtos_CategoriaId",
            //    table: "Produtos",
            //    column: "CategoriaId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Usuarios_PermissaoId",
            //    table: "Usuarios",
            //    column: "PermissaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
