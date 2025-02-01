using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGES.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRelacionamentoGarantia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garantias_Os_OrdensId",
                table: "Garantias");

            migrationBuilder.DropIndex(
                name: "IX_Garantias_OrdensId",
                table: "Garantias");

            migrationBuilder.DropColumn(
                name: "OrdensId",
                table: "Garantias");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$EqUikjHYKXlQLVhjrSWIsO/Mre9t69t3/o/r1./Sp6bH4PRItridO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdensId",
                table: "Garantias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$QsouWLoXDta.Jz/EElYKh.6SHZB3Po0/qBPQPOQYPSkvSJ3MVJL3q");

            migrationBuilder.CreateIndex(
                name: "IX_Garantias_OrdensId",
                table: "Garantias",
                column: "OrdensId");

            migrationBuilder.AddForeignKey(
                name: "FK_Garantias_Os_OrdensId",
                table: "Garantias",
                column: "OrdensId",
                principalTable: "Os",
                principalColumn: "id_os",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
