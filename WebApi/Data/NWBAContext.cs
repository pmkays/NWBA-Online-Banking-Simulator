﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApi.Models;

namespace WebApi.Data
{
    public partial class NWBAContext : DbContext
    {
        public NWBAContext()
        {
        }

        public NWBAContext(DbContextOptions<NWBAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BillPay> BillPay { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Payee> Payee { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source = (localdb)\\mssqllocaldb; Database = Bank;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.CustomerId);
            });

            modelBuilder.Entity<BillPay>(entity =>
            {
                entity.HasIndex(e => e.AccountNumber);

                entity.HasIndex(e => e.PayeeId);

                entity.Property(e => e.Status).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasIndex(e => e.CustomerId)
                    .IsUnique();

                entity.Property(e => e.BlockTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Status).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(e => e.AccountNumber);

                entity.HasIndex(e => e.DestinationAccountNumber);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
