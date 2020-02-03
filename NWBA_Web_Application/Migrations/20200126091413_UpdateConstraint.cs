using Microsoft.EntityFrameworkCore.Migrations;

namespace NWBA_Web_Application.Migrations
{
    public partial class UpdateConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCheckConstraint(
                name: "CH_PayeeID2",
                table: "Payee",
                sql: "PayeeID >= 1000");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_CustomerID2",
                table: "Customer",
                sql: "CustomerID >= 1000");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_CustomerPostCode2",
                table: "Customer",
                sql: "PostCode >= 1000");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_BillPayID2",
                table: "BillPay",
                sql: "BillPayID >= 1000");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_BillAmount",
                table: "BillPay",
                sql: "Amount > 0");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_AccountNumber2",
                table: "Account",
                sql: "AccountNumber >= 1000");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_PayeeID2",
                table: "Payee");

            migrationBuilder.DropCheckConstraint(
                name: "CH_CustomerID2",
                table: "Customer");

            migrationBuilder.DropCheckConstraint(
                name: "CH_CustomerPostCode2",
                table: "Customer");

            migrationBuilder.DropCheckConstraint(
                name: "CH_BillPayID2",
                table: "BillPay");

            migrationBuilder.DropCheckConstraint(
                name: "CH_BillAmount",
                table: "BillPay");

            migrationBuilder.DropCheckConstraint(
                name: "CH_AccountNumber2",
                table: "Account");
        }
    }
}
