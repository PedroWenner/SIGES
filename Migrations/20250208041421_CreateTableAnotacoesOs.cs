using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableAnotacoesOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnotacoesOs",
                columns: table => new
                {
                    id_anotacoes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    anotacao = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_hora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    os_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnotacoesOs", x => x.id_anotacoes);
                    table.ForeignKey(
                        name: "FK_AnotacoesOs_Os_os_id",
                        column: x => x.os_id,
                        principalTable: "Os",
                        principalColumn: "id_os",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$p5049BO0OCr3QE8w6acnpOZj6N1WUwyTtUcEvRzUs0UcW5LOBf7dC");

            migrationBuilder.CreateIndex(
                name: "IX_AnotacoesOs_os_id",
                table: "AnotacoesOs",
                column: "os_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnotacoesOs");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$MIGU6WC.3RIB3KqRVnX.g.CG6x0cfD9m8m1mLuNrWwOHbslQccsvu");
        }
    }
}
