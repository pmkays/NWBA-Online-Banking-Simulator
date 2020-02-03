using Microsoft.EntityFrameworkCore.Migrations;

namespace NWBA_Web_Application.Migrations
{
    public partial class UpdateConstraint2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCheckConstraint(
                name: "CH_TransactionType",
                table: "Transaction",
                sql: "TransactionType in ('B','S','T','W','D')");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_State",
                table: "Customer",
                sql: "State in ('VIC','QLD','NSW','NT','TAS','ACT','SA','WA')");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_PeriodType",
                table: "BillPay",
                sql: "Period in ('A','S','Q','M')");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_ScheduleDate",
                table: "BillPay",
                sql: "ScheduleDate > SYSDATETIME()");

            migrationBuilder.CreateCheckConstraint(
                name: "CH_AccountType",
                table: "Account",
                sql: "AccountType in ('C','S')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CH_TransactionType",
                table: "Transaction");

            migrationBuilder.DropCheckConstraint(
                name: "CH_State",
                table: "Customer");

            migrationBuilder.DropCheckConstraint(
                name: "CH_PeriodType",
                table: "BillPay");

            migrationBuilder.DropCheckConstraint(
                name: "CH_ScheduleDate",
                table: "BillPay");

            migrationBuilder.DropCheckConstraint(
                name: "CH_AccountType",
                table: "Account");
        }
    }
}
