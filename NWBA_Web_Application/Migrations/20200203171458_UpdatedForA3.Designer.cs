﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NWBA_Web_Application.Data;

namespace NWBA_Web_Application.Migrations
{
    [DbContext(typeof(NWBAContext))]
    [Migration("20200203171458_UpdatedForA3")]
    partial class UpdatedForA3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NWBA_Web_Application.Models.Account", b =>
                {
                    b.Property<int>("AccountNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("money");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AccountNumber");

                    b.HasIndex("CustomerID");

                    b.ToTable("Account");

                    b.HasCheckConstraint("CH_Account_Balance", "Balance >= 0");

                    b.HasCheckConstraint("CH_AccountNumber", "AccountNumber <= 9999");

                    b.HasCheckConstraint("CH_AccountNumber2", "AccountNumber => 1000");

                    b.HasCheckConstraint("CH_AccountType", "AccountType in ('C','S')");
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.BillPay", b =>
                {
                    b.Property<int>("BillPayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PayeeID")
                        .HasColumnType("int");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.Property<DateTime>("ScheduleDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BillPayID");

                    b.HasIndex("AccountNumber");

                    b.HasIndex("PayeeID");

                    b.ToTable("BillPay");

                    b.HasCheckConstraint("CH_BillPayID", "BillPayID <= 9999");

                    b.HasCheckConstraint("CH_BillPayID2", "BillPayID => 1000");

                    b.HasCheckConstraint("CH_BillAmount", "Amount > 0");

                    b.HasCheckConstraint("CH_PeriodType", "Period in ('A','S','Q','M')");

                    b.HasCheckConstraint("CH_ScheduleDate", "ScheduleDate > SYSDATETIME()");
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(14)")
                        .HasMaxLength(14);

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("TFN")
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.HasKey("CustomerID");

                    b.ToTable("Customer");

                    b.HasCheckConstraint("CH_CustomerID", "CustomerID <= 9999");

                    b.HasCheckConstraint("CH_CustomerID2", "CustomerID => 1000");

                    b.HasCheckConstraint("CH_CustomerPostCode", "PostCode <= 9999");

                    b.HasCheckConstraint("CH_CustomerPostCode2", "PostCode => 1000");

                    b.HasCheckConstraint("CH_State", "State in ('VIC','QLD','NSW','NT','TAS','ACT','SA','WA')");
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Login", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("BlockTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.HasIndex("CustomerID")
                        .IsUnique();

                    b.ToTable("Login");
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Payee", b =>
                {
                    b.Property<int>("PayeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("PayeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PayeeID");

                    b.ToTable("Payee");

                    b.HasCheckConstraint("CH_PayeePostCode", "PostCode <= 9999");

                    b.HasCheckConstraint("CH_PayeeID", "PayeeID <= 9999");

                    b.HasCheckConstraint("CH_PayeeID2", "PayeeID => 1000");
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("DestinationAccountNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.HasKey("TransactionID");

                    b.HasIndex("AccountNumber");

                    b.HasIndex("DestinationAccountNumber");

                    b.ToTable("Transaction");

                    b.HasCheckConstraint("CH_TransactionID", "TransactionID <= 9999");

                    b.HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");

                    b.HasCheckConstraint("CH_TransactionType", "TransactionType in ('B','S','T','W','D')");
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Account", b =>
                {
                    b.HasOne("NWBA_Web_Application.Models.Customer", "Customer")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.BillPay", b =>
                {
                    b.HasOne("NWBA_Web_Application.Models.Account", "Account")
                        .WithMany("BillPays")
                        .HasForeignKey("AccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NWBA_Web_Application.Models.Payee", "Payee")
                        .WithMany("BillPays")
                        .HasForeignKey("PayeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Login", b =>
                {
                    b.HasOne("NWBA_Web_Application.Models.Customer", "Customer")
                        .WithOne("Login")
                        .HasForeignKey("NWBA_Web_Application.Models.Login", "CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NWBA_Web_Application.Models.Transaction", b =>
                {
                    b.HasOne("NWBA_Web_Application.Models.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NWBA_Web_Application.Models.Account", "DestinationAccount")
                        .WithMany()
                        .HasForeignKey("DestinationAccountNumber");
                });
#pragma warning restore 612, 618
        }
    }
}