using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NWBA_Web_Application.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(maxLength: 50, nullable: false),
                    TFN = table.Column<string>(maxLength: 11, nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 40, nullable: true),
                    State = table.Column<string>(maxLength: 20, nullable: true),
                    PostCode = table.Column<string>(maxLength: 10, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                    table.CheckConstraint("CH_CustomerID", "CustomerID <= 9999");
                    table.CheckConstraint("CH_CustomerPostCode", "PostCode <= 9999");
                });

            migrationBuilder.CreateTable(
                name: "Payee",
                columns: table => new
                {
                    PayeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayeeName = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 40, nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(maxLength: 10, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payee", x => x.PayeeID);
                    table.CheckConstraint("CH_PayeePostCode", "PostCode <= 9999");
                    table.CheckConstraint("CH_PayeeID", "PayeeID <= 9999");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountType = table.Column<string>(maxLength: 1, nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(type: "money", nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountNumber);
                    table.CheckConstraint("CH_Account_Balance", "Balance >= 0");
                    table.CheckConstraint("CH_AccountNumber", "AccountNumber <= 9999");
                    table.ForeignKey(
                        name: "FK_Account_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    UserID = table.Column<string>(maxLength: 50, nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 64, nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Login_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPay",
                columns: table => new
                {
                    BillPayID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(nullable: false),
                    PayeeID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    Period = table.Column<string>(maxLength: 1, nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPay", x => x.BillPayID);
                    table.CheckConstraint("CH_BillPayID", "BillPayID <= 9999");
                    table.ForeignKey(
                        name: "FK_BillPay_Account_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPay_Payee_PayeeID",
                        column: x => x.PayeeID,
                        principalTable: "Payee",
                        principalColumn: "PayeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(maxLength: 1, nullable: false),
                    AccountNumber = table.Column<int>(nullable: false),
                    DestinationAccountNumber = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    Comment = table.Column<string>(maxLength: 255, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionID);
                    table.CheckConstraint("CH_TransactionID", "TransactionID <= 9999");
                    table.CheckConstraint("CH_Transaction_Amount", "Amount > 0");
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_DestinationAccountNumber",
                        column: x => x.DestinationAccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerID",
                table: "Account",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_BillPay_AccountNumber",
                table: "BillPay",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BillPay_PayeeID",
                table: "BillPay",
                column: "PayeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Login_CustomerID",
                table: "Login",
                column: "CustomerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountNumber",
                table: "Transaction",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestinationAccountNumber",
                table: "Transaction",
                column: "DestinationAccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillPay");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Payee");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
