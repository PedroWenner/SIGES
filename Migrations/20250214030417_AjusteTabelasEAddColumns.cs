using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTabelasEAddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lancamento",
                table: "Os",
                newName: "lancamento_id");

            migrationBuilder.AddColumn<int>(
                name: "os_id",
                table: "Lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$36myZ.seixfU1tH0UGDnV.NXr/56p39VLwc/HC3RF96JzpbqnsvaC");

            migrationBuilder.CreateIndex(
                name: "IX_Os_lancamento_id",
                table: "Os",
                column: "lancamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_os_id",
                table: "Lancamentos",
                column: "os_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Os_os_id",
                table: "Lancamentos",
                column: "os_id",
                principalTable: "Os",
                principalColumn: "id_os",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Os_Lancamentos_lancamento_id",
                table: "Os",
                column: "lancamento_id",
                principalTable: "Lancamentos",
                principalColumn: "idLancamentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Os_os_id",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Os_Lancamentos_lancamento_id",
                table: "Os");

            migrationBuilder.DropIndex(
                name: "IX_Os_lancamento_id",
                table: "Os");

            migrationBuilder.DropIndex(
                name: "IX_Lancamentos_os_id",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "os_id",
                table: "Lancamentos");

            migrationBuilder.RenameColumn(
                name: "lancamento_id",
                table: "Os",
                newName: "lancamento");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$z2YzO8tv10GO3fvCuMmyjOLPUqKGm/hJ/3FLN7CluTFtR7UKK5Wr.");
        }
    }
}
