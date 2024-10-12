using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transazioni.Infrastructure.Migrations
{
    public partial class Add_AccountType_Instead_Of_Boolean_IsPatrimonial_In_Accounts_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPatrimonial",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "Accounts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsPatrimonial",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
