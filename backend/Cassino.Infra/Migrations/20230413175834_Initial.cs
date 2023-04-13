using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cassino.Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administradores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desativado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CriadoPor = table.Column<int>(type: "int", nullable: true),
                    CriadoPorAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AtualizadoPor = table.Column<int>(type: "int", nullable: true),
                    AtualizadoPorAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administradores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NomeSocial = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    Senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desativado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CriadoPor = table.Column<int>(type: "int", nullable: true),
                    CriadoPorAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AtualizadoPor = table.Column<int>(type: "int", nullable: true),
                    AtualizadoPorAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administradores");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
