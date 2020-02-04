using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApi.Models;

namespace WebApi.Data
{
    public partial class s3788210Context : DbContext
    {
        public s3788210Context()
        {
        }

        public s3788210Context(DbContextOptions<s3788210Context> options)
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
                optionsBuilder.UseSqlServer("Data Source=wdt2020.australiasoutheast.cloudapp.azure.com;uid=s3788210;database=s3788210;pwd=abc123");
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
