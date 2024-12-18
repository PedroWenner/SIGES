using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Data", "Nome", "Permissoes", "Situacao" },
                values: new object[] { 1, null, "Master", null, 1 });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Bairro", "Cep", "Cidade", "Cpf", "DataExpiracao", "Email", "Estado", "ImagemBlob", "Master", "Nome", "NumeroRua", "PermissaoId", "RgNumero", "Rua", "Senha", "Situacao", "Telefone" },
                values: new object[] { 1, null, null, null, "000.000.000-00", null, "teste@teste.com", null, null, "S", "Admin", null, 1, null, null, "$2a$11$kZIc2o.Z5v4PDZEi7eduquIm2JxOC9dFZYzqbYkrVI/aYFu8.9j36", 1, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
