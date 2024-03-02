using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transazioni.Infrastructure.Migrations
{
    public partial class Movements_IsImported_Peridiocity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImported",
                table: "Movements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Peridiocity",
                table: "Movements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImported",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "Peridiocity",
                table: "Movements");

            migrationBuilder.AddColumn<Guid>(
                name: "Money_MovementsId",
                table: "Movements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Movements_Money_MovementsId",
                table: "Movements",
                column: "Money_MovementsId",
                principalTable: "Movements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
