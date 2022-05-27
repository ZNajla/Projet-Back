using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class mg14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Detail_Processus",
                table: "Detail_Processus");

            migrationBuilder.DropIndex(
                name: "IX_Detail_Processus_UserId",
                table: "Detail_Processus");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Detail_Processus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Detail_Processus",
                table: "Detail_Processus",
                columns: new[] { "UserId", "ProcessusId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Detail_Processus",
                table: "Detail_Processus");

            migrationBuilder.AddColumn<string>(
                name: "ID",
                table: "Detail_Processus",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Detail_Processus",
                table: "Detail_Processus",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Detail_Processus_UserId",
                table: "Detail_Processus",
                column: "UserId");
        }
    }
}
