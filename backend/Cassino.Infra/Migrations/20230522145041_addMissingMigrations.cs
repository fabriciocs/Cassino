using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cassino.Infra.Migrations
{
    public partial class addMissingMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoRecuperacaoSenha",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TempoExpiracaoDoCodigo",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoRecuperacaoSenha",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TempoExpiracaoDoCodigo",
                table: "Usuarios");
        }
    }
}
