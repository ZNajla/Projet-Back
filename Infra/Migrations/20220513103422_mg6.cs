using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class mg6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TacheUser_Tache_TachesID",
                table: "TacheUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tache",
                table: "Tache");

            migrationBuilder.RenameTable(
                name: "Tache",
                newName: "Taches");

            migrationBuilder.AddColumn<string>(
                name: "TypesID",
                table: "Documents",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Taches",
                table: "Taches",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypesID",
                table: "Documents",
                column: "TypesID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Types_TypesID",
                table: "Documents",
                column: "TypesID",
                principalTable: "Types",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TacheUser_Taches_TachesID",
                table: "TacheUser",
                column: "TachesID",
                principalTable: "Taches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Types_TypesID",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_TacheUser_Taches_TachesID",
                table: "TacheUser");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Documents_TypesID",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Taches",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "TypesID",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "Taches",
                newName: "Tache");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tache",
                table: "Tache",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TacheUser_Tache_TachesID",
                table: "TacheUser",
                column: "TachesID",
                principalTable: "Tache",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
