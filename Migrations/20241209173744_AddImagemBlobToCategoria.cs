using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AddImagemBlobToCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "CaminhoImagem",
            //    table: "Categorias");

            //migrationBuilder.AddColumn<byte[]>(
            //    name: "ImagemBlob",
            //    table: "Categorias",
            //    type: "varbinary(max)",
            //    nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemBlob",
                table: "Categorias");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoImagem",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
