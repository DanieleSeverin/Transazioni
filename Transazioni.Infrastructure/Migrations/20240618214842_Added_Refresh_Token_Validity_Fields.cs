using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transazioni.Infrastructure.Migrations
{
    public partial class Added_Refresh_Token_Validity_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InvalidatedAt",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvalidityReason",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvalidatedAt",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "InvalidityReason",
                table: "RefreshToken");
        }
    }
}
