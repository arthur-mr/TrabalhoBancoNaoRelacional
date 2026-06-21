using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiFurb.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Inicial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Identificacao",
                table: "COMANDA",
                newName: "IDENTIFICACAO");

            migrationBuilder.AlterColumn<Guid>(
                name: "CLIENTE_FK",
                table: "COMANDA",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IDENTIFICACAO",
                table: "COMANDA",
                newName: "Identificacao");

            migrationBuilder.AlterColumn<Guid>(
                name: "CLIENTE_FK",
                table: "COMANDA",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
