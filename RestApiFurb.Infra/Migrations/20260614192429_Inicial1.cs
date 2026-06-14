using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiFurb.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Inicial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRECO = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CODIGO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CODIGO_BARRAS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CATEGORIA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QUANTIDADE_ESTOQUE = table.Column<int>(type: "int", nullable: false),
                    ATIVO = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    DATA_ATIVACAO = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DATA_DESATIVACAO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TELEFONE = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ATIVO = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    DATA_ATIVACAO = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DATA_DESATIVACAO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "COMANDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    USUARIO_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ATIVO = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    DATA_ATIVACAO = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DATA_DESATIVACAO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_COMANDA_USUARIO_USUARIO_FK",
                        column: x => x.USUARIO_FK,
                        principalTable: "USUARIO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_COMANDA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    COMANDA_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRODUTO_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_COMANDA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUTO_COMANDA_COMANDA_COMANDA_FK",
                        column: x => x.COMANDA_FK,
                        principalTable: "COMANDA",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PRODUTO_COMANDA_PRODUTO_PRODUTO_FK",
                        column: x => x.PRODUTO_FK,
                        principalTable: "PRODUTO",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMANDA_USUARIO_FK",
                table: "COMANDA",
                column: "USUARIO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_COMANDA_COMANDA_FK",
                table: "PRODUTO_COMANDA",
                column: "COMANDA_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_COMANDA_PRODUTO_FK",
                table: "PRODUTO_COMANDA",
                column: "PRODUTO_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUTO_COMANDA");

            migrationBuilder.DropTable(
                name: "COMANDA");

            migrationBuilder.DropTable(
                name: "PRODUTO");

            migrationBuilder.DropTable(
                name: "USUARIO");
        }
    }
}
