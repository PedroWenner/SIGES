using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDataCadastroToTableClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_cadastro",
                table: "Clientes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$fkZmc1rMzzJ5ZJTf1fVtiuQE/iZj2DvbQhcWa2iUA5vWUjbN7xG3m");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_cadastro",
                table: "Clientes");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$hQQH6xJApptoZhbqUUzH7u8eFNjz3ta7NEFLxQMVvOrtvQBSKekNS");
        }
    }
}
