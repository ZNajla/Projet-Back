using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class m7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date_fin",
                table: "Steps");

            migrationBuilder.RenameColumn(
                name: "Date_debut",
                table: "Steps",
                newName: "date_debut");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_debut",
                table: "Steps",
                newName: "Date_debut");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_fin",
                table: "Steps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
