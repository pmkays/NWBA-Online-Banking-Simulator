using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Data
{
    public class NWBAContext : DbContext
    {
        public NWBAContext(DbContextOptions<NWBAContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Payee> Payee { get; set; }
        public DbSet<BillPay> BillPay { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Relationships

            builder.Entity<Customer>().HasMany(x => x.Accounts).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerID);
            builder.Entity<Customer>().HasOne<Login>(x => x.Login).WithOne(x => x.Customer).HasForeignKey<Login>(x => x.CustomerID);

            builder.Entity<Account>().HasMany(x => x.Transactions).WithOne(x => x.Account).HasForeignKey(x => x.AccountNumber);
            builder.Entity<Account>().HasMany(x => x.BillPays).WithOne(x => x.Account).HasForeignKey(x => x.AccountNumber);

            builder.Entity<BillPay>().HasOne(x => x.Payee).WithMany(x => x.BillPays).HasForeignKey(x => x.PayeeID);

            builder.Entity<Transaction>().HasOne(x => x.DestinationAccount).WithMany().HasForeignKey(x => x.DestinationAccountNumber);

            //Constraints

            builder.Entity<Customer>().HasCheckConstraint("CH_CustomerID", "CustomerID <= 9999").
                HasCheckConstraint("CH_CustomerID2", "CustomerID => 1000").
                HasCheckConstraint("CH_CustomerPostCode","PostCode <= 9999").
                HasCheckConstraint("CH_CustomerPostCode2", "PostCode => 1000").
                HasCheckConstraint("CH_State", "State in ('VIC','QLD','NSW','NT','TAS','ACT','SA','WA')");
            builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0").
                HasCheckConstraint("CH_AccountNumber", "AccountNumber <= 9999").
                HasCheckConstraint("CH_AccountNumber2", "AccountNumber => 1000").
                HasCheckConstraint("CH_AccountType", "AccountType in ('C','S')");

            builder.Entity<BillPay>().
                HasCheckConstraint("CH_BillPayID", "BillPayID <= 9999").
                HasCheckConstraint("CH_BillPayID2", "BillPayID => 1000").
                HasCheckConstraint("CH_BillAmount", "Amount > 0").
                HasCheckConstraint("CH_PeriodType", "Period in ('A','S','Q','M')").
                HasCheckConstraint("CH_ScheduleDate", "ScheduleDate > SYSDATETIME()");

            builder.Entity<Payee>().
                HasCheckConstraint("CH_PayeePostCode", "PostCode <= 9999").
                HasCheckConstraint("CH_PayeeID", "PayeeID <= 9999").
                HasCheckConstraint("CH_PayeeID2", "PayeeID => 1000");

            builder.Entity<Transaction>().HasCheckConstraint("CH_TransactionID", "TransactionID <= 9999").
                HasCheckConstraint("CH_Transaction_Amount", "Amount > 0").
                HasCheckConstraint("CH_TransactionType", "TransactionType in ('B','S','T','W','D')");
        }
    }

}
