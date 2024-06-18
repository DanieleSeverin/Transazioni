using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transazioni.Infrastructure.Migrations
{
    public partial class Added_Refresh_Token_Valid_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExireAt",
                table: "RefreshToken",
                newName: "ExpireAt");

            migrationBuilder.AddColumn<bool>(
                name: "Valid",
                table: "RefreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valid",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "ExpireAt",
                table: "RefreshToken",
                newName: "ExireAt");
        }
    }
}
