using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableProdutosOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "sub-total",
            //    table: "ServicosOs",
            //    newName: "sub_total");

            migrationBuilder.AlterColumn<int>(
                name: "quantidade",
                table: "ServicosOs",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProdutosOs",
                columns: table => new
                {
                    idProdutos_os = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    preco = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    os_id = table.Column<int>(type: "int", nullable: false),
                    produtos_id = table.Column<int>(type: "int", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosOs", x => x.idProdutos_os);
                    table.ForeignKey(
                        name: "FK_ProdutosOs_Os_os_id",
                        column: x => x.os_id,
                        principalTable: "Os",
                        principalColumn: "id_os",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosOs_Produtos_produtos_id",
                        column: x => x.produtos_id,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$YQwGsKgUfX0lbcXcjreRZelxiiJFwfD3Gv1r3FeBpi8NLoVhXMR/2");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOs_os_id",
                table: "ProdutosOs",
                column: "os_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOs_produtos_id",
                table: "ProdutosOs",
                column: "produtos_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutosOs");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "ServicosOs",
                newName: "sub-total");

            migrationBuilder.AlterColumn<double>(
                name: "quantidade",
                table: "ServicosOs",
                type: "double",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$M23G/larOVLwytbWeWvfHOM8aashiP/zc2XIpPuB7kgbzHpuQuMWC");
        }
    }
}
