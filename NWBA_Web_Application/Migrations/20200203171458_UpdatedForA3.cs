using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NWBA_Web_Application.Migrations
{
    public partial class UpdatedForA3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_State",
                table: "Customer");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_State",
                table: "Customer",
                sql: "State in ('VIC','QLD','NSW','NT','TAS','ACT','SA','WA')");

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockTime",
                table: "Login",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LoginAttempts",
                table: "Login",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Login",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customer",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BillPay",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_State",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BlockTime",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "LoginAttempts",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BillPay");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_State",
                table: "Customer",
                sql: "State in ('VIC','QLD','NSW','NT','TAS','ACT','SA','WA)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customer",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14,
                oldNullable: true);
        }
    }
}
