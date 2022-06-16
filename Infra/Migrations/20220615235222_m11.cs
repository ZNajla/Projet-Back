using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class m11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroState",
                table: "DocumentState",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DocumentState",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CurrentNumberState",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentState_UserId",
                table: "DocumentState",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentState_AspNetUsers_UserId",
                table: "DocumentState",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentState_AspNetUsers_UserId",
                table: "DocumentState");

            migrationBuilder.DropIndex(
                name: "IX_DocumentState_UserId",
                table: "DocumentState");

            migrationBuilder.DropColumn(
                name: "NumeroState",
                table: "DocumentState");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DocumentState");

            migrationBuilder.DropColumn(
                name: "CurrentNumberState",
                table: "Documents");
        }
    }
}
