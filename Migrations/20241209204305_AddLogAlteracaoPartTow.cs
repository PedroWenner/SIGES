using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AddLogAlteracaoPartTow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "LogsOperacoes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Entidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        EntidadeId = table.Column<int>(type: "int", nullable: false),
            //        Alteracoes = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LogsOperacoes", x => x.Id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsOperacoes");
        }
    }
}
