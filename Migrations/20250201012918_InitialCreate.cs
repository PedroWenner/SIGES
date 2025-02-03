using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
            //migrationBuilder.AlterDatabase()
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Categorias",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Descricao = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ImagemBlob = table.Column<byte[]>(type: "longblob", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categorias", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Clientes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Documento = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Nome = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Sexo = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        PessoaFisica = table.Column<int>(type: "int", nullable: false),
            //        Contato = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Telefone = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Celular = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Email = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Senha = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Fornecedor = table.Column<int>(type: "int", nullable: false),
            //        Cep = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Rua = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        NumeroRua = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Complemento = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Bairro = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cidade = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Estado = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Clientes", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Configuracoes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Config = table.Column<string>(type: "varchar(255)", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Valor = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Configuracoes", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "EmailQueue",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        To = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cc = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Bcc = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Message = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Status = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        Headers = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EmailQueue", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Emitente",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Nome = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cnpj = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Ie = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cep = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Rua = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Numero = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Bairro = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cidade = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Uf = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Telefone = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Email = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ImagemBlob = table.Column<byte[]>(type: "longblob", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Emitente", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "LogsOperacoes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Usuario = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        Entidade = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        EntidadeId = table.Column<int>(type: "int", nullable: false),
            //        Alteracoes = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        TipoOperacao = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LogsOperacoes", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Permissoes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Nome = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Permissoes = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Situacao = table.Column<int>(type: "int", nullable: true),
            //        Data = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Permissoes", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Produtos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        CodigoBarras = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Descricao = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        TipoMovimento = table.Column<int>(type: "int", nullable: false),
            //        PrecoCompra = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
            //        MargemLucro = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
            //        PrecoVenda = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
            //        UnidadeMedida = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Estoque = table.Column<int>(type: "int", nullable: false),
            //        EstoqueMin = table.Column<int>(type: "int", nullable: true),
            //        ImagemBlob = table.Column<byte[]>(type: "longblob", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Produtos", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Servicos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Nome = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Descricao = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Valor = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Servicos", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Usuarios",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Nome = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        RgNumero = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cpf = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Telefone = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Email = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Senha = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cep = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Rua = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        NumeroRua = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Bairro = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Cidade = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Estado = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        DataExpiracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        Situacao = table.Column<int>(type: "int", nullable: true),
            //        ImagemBlob = table.Column<byte[]>(type: "longblob", nullable: true),
            //        PermissaoId = table.Column<int>(type: "int", nullable: false),
            //        Master = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Usuarios", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Usuarios_Permissoes_PermissaoId",
            //            column: x => x.PermissaoId,
            //            principalTable: "Permissoes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "cobrancas",
            //    columns: table => new
            //    {
            //        idCobranca = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        charge_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        conditional_discount_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        custom_id = table.Column<int>(type: "int", nullable: true),
            //        expire_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        message = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        payment_method = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        payment_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        request_delivery_address = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        status = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        total = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        barcode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        payment_gateway = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        payment = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        pdf = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        vendas_id = table.Column<int>(type: "int", nullable: true),
            //        os_id = table.Column<int>(type: "int", nullable: true),
            //        clientes_id = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cobrancas", x => x.idCobranca);
            //        table.ForeignKey(
            //            name: "FK_cobrancas_Clientes_clientes_id",
            //            column: x => x.clientes_id,
            //            principalTable: "Clientes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Garantias",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        DataGarantia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        RefGarantia = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        TextoGarantia = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        UsuarioId = table.Column<int>(type: "int", nullable: true),
            //        OrdensId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Garantias", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Garantias_Usuarios_UsuarioId",
            //            column: x => x.UsuarioId,
            //            principalTable: "Usuarios",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Os",
            //    columns: table => new
            //    {
            //        id_os = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        data_inicial = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        data_final = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        garantia = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        descricao_produto = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        defeito = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        status = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        observacoes = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        laudo_tecnico = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        valor_total = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
            //        desconto = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
            //        valor_desconto = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
            //        tipo_desconto = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        clientes_id = table.Column<int>(type: "int", nullable: false),
            //        usuarios_id = table.Column<int>(type: "int", nullable: false),
            //        lancamento = table.Column<int>(type: "int", nullable: true),
            //        faturado = table.Column<int>(type: "int", nullable: true),
            //        garantias_id = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Os", x => x.id_os);
            //        table.ForeignKey(
            //            name: "FK_Os_Clientes_clientes_id",
            //            column: x => x.clientes_id,
            //            principalTable: "Clientes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Os_Garantias_garantias_id",
            //            column: x => x.garantias_id,
            //            principalTable: "Garantias",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "FK_Os_Usuarios_usuarios_id",
            //            column: x => x.usuarios_id,
            //            principalTable: "Usuarios",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "ServicosOs",
            //    columns: table => new
            //    {
            //        id_servicos = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        servico = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        quantidade = table.Column<double>(type: "double", nullable: true),
            //        preco = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
            //        os_id = table.Column<int>(type: "int", nullable: false),
            //        servicos_id = table.Column<int>(type: "int", nullable: false),
            //        subtotal = table.Column<decimal>(name: "sub-total", type: "decimal(15,2)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ServicosOs", x => x.id_servicos);
            //        table.ForeignKey(
            //            name: "FK_ServicosOs_Os_os_id",
            //            column: x => x.os_id,
            //            principalTable: "Os",
            //            principalColumn: "id_os",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ServicosOs_Servicos_servicos_id",
            //            column: x => x.servicos_id,
            //            principalTable: "Servicos",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "ItensVenda",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        SubTotal = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Quantidade = table.Column<int>(type: "int", nullable: true),
            //        Preco = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
            //        VendaId = table.Column<int>(type: "int", nullable: false),
            //        ProdutoId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ItensVenda", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ItensVenda_Produtos_ProdutoId",
            //            column: x => x.ProdutoId,
            //            principalTable: "Produtos",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Lancamentos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        Descricao = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Valor = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
            //        DataVencimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
            //        Baixado = table.Column<int>(type: "int", nullable: true),
            //        ClienteFornecedor = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        FormaPgto = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Tipo = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        AnexoBlob = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ClienteId = table.Column<int>(type: "int", nullable: true),
            //        CategoriaId = table.Column<int>(type: "int", nullable: true),
            //        VendaId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Lancamentos", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Lancamentos_Categorias_CategoriaId",
            //            column: x => x.CategoriaId,
            //            principalTable: "Categorias",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Lancamentos_Clientes_ClienteId",
            //            column: x => x.ClienteId,
            //            principalTable: "Clientes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Vendas",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        DataVenda = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        ValorTotal = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
            //        Desconto = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
            //        ValorDesconto = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
            //        TipoDesconto = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Faturado = table.Column<int>(type: "int", nullable: true),
            //        Observacao = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ObservacaoCliente = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ClienteId = table.Column<int>(type: "int", nullable: false),
            //        UsuarioId = table.Column<int>(type: "int", nullable: false),
            //        LancamentoId = table.Column<int>(type: "int", nullable: true),
            //        Status = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        GarantiaDias = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Vendas", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Vendas_Clientes_ClienteId",
            //            column: x => x.ClienteId,
            //            principalTable: "Clientes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Vendas_Lancamentos_LancamentoId",
            //            column: x => x.LancamentoId,
            //            principalTable: "Lancamentos",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Vendas_Usuarios_UsuarioId",
            //            column: x => x.UsuarioId,
            //            principalTable: "Usuarios",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.InsertData(
            //    table: "Permissoes",
            //    columns: new[] { "Id", "Data", "Nome", "Permissoes", "Situacao" },
            //    values: new object[] { 1, null, "Master", null, 1 });

            //migrationBuilder.InsertData(
            //    table: "Usuarios",
            //    columns: new[] { "Id", "Bairro", "Cep", "Cidade", "Cpf", "DataExpiracao", "Email", "Estado", "ImagemBlob", "Master", "Nome", "NumeroRua", "PermissaoId", "RgNumero", "Rua", "Senha", "Situacao", "Telefone" },
            //    values: new object[] { 1, null, null, null, "000.000.000-00", null, "teste@teste.com", null, null, "S", "Admin", null, 1, null, null, "$2a$11$QsouWLoXDta.Jz/EElYKh.6SHZB3Po0/qBPQPOQYPSkvSJ3MVJL3q", 1, null });

            //migrationBuilder.CreateIndex(
            //    name: "IX_cobrancas_clientes_id",
            //    table: "cobrancas",
            //    column: "clientes_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cobrancas_os_id",
            //    table: "cobrancas",
            //    column: "os_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cobrancas_vendas_id",
            //    table: "cobrancas",
            //    column: "vendas_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Configuracoes_Config",
            //    table: "Configuracoes",
            //    column: "Config",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Garantias_OrdensId",
            //    table: "Garantias",
            //    column: "OrdensId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Garantias_UsuarioId",
            //    table: "Garantias",
            //    column: "UsuarioId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItensVenda_ProdutoId",
            //    table: "ItensVenda",
            //    column: "ProdutoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItensVenda_VendaId",
            //    table: "ItensVenda",
            //    column: "VendaId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lancamentos_CategoriaId",
            //    table: "Lancamentos",
            //    column: "CategoriaId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lancamentos_ClienteId",
            //    table: "Lancamentos",
            //    column: "ClienteId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lancamentos_VendaId",
            //    table: "Lancamentos",
            //    column: "VendaId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Os_clientes_id",
            //    table: "Os",
            //    column: "clientes_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Os_garantias_id",
            //    table: "Os",
            //    column: "garantias_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Os_usuarios_id",
            //    table: "Os",
            //    column: "usuarios_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServicosOs_os_id",
            //    table: "ServicosOs",
            //    column: "os_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ServicosOs_servicos_id",
            //    table: "ServicosOs",
            //    column: "servicos_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Usuarios_PermissaoId",
            //    table: "Usuarios",
            //    column: "PermissaoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vendas_ClienteId",
            //    table: "Vendas",
            //    column: "ClienteId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vendas_LancamentoId",
            //    table: "Vendas",
            //    column: "LancamentoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vendas_UsuarioId",
            //    table: "Vendas",
            //    column: "UsuarioId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_cobrancas_Os_os_id",
            //    table: "cobrancas",
            //    column: "os_id",
            //    principalTable: "Os",
            //    principalColumn: "id_os",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_cobrancas_Vendas_vendas_id",
            //    table: "cobrancas",
            //    column: "vendas_id",
            //    principalTable: "Vendas",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Garantias_Os_OrdensId",
            //    table: "Garantias",
            //    column: "OrdensId",
            //    principalTable: "Os",
            //    principalColumn: "id_os",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ItensVenda_Vendas_VendaId",
            //    table: "ItensVenda",
            //    column: "VendaId",
            //    principalTable: "Vendas",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Lancamentos_Vendas_VendaId",
            //    table: "Lancamentos",
            //    column: "VendaId",
            //    principalTable: "Vendas",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Clientes_ClienteId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Os_Clientes_clientes_id",
                table: "Os");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Clientes_ClienteId",
                table: "Vendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Garantias_Os_OrdensId",
                table: "Garantias");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Vendas_VendaId",
                table: "Lancamentos");

            migrationBuilder.DropTable(
                name: "cobrancas");

            migrationBuilder.DropTable(
                name: "Configuracoes");

            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "Emitente");

            migrationBuilder.DropTable(
                name: "ItensVenda");

            migrationBuilder.DropTable(
                name: "LogsOperacoes");

            migrationBuilder.DropTable(
                name: "ServicosOs");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Os");

            migrationBuilder.DropTable(
                name: "Garantias");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Permissoes");
        }
    }
}
