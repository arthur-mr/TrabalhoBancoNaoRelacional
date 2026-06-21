using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiFurb.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Inicial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "STATUS",
                table: "COMANDA",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "COMANDA");
        }
    }
}
