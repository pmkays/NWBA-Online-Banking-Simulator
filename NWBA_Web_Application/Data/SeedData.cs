using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NWBA_Web_Application.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Data
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new NWBAContext(serviceProvider.GetRequiredService<DbContextOptions<NWBAContext>>());

            //Look for customers.
            if (context.Customer.Any())
                return; // DB has already been seeded.

            AddCustomers(context);
            AddLogins(context);
            AddAccounts(context);
            AddTransactions(context);
        }

        private static void AddCustomers(NWBAContext context)
        {
            context.Customer.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    CustomerName = "Matthew Bolger",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    PostCode = "3000",
                    TFN = "1337",
                    State = "VIC",
                    Phone = "(61)-94491023"
                },
                new Customer
                {
                    CustomerID = 2200,
                    CustomerName = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    PostCode = "3005",
                    TFN = "43770",
                    State = "VIC",
                    Phone = "(61)-93906170"
                },
                new Customer
                {
                    CustomerID = 2300,
                    CustomerName = "Shekhar Kalra",
                    Phone = "(61)-93906170"
                });

            context.Database.OpenConnection();

            try
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Customer ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Customer OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }

        private static void AddLogins(NWBAContext context)
        {
            const string format = "dd/MM/yyyy hh:mm:ss tt";
            context.Login.AddRange(
                new Login
                {
                    UserID = "12345678",
                    CustomerID = 2100,
                    PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Login
                {
                    UserID = "38074569",
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)

                },
                new Login
                {
                    UserID = "17963428",
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            context.SaveChanges();
        }

        private static void AddAccounts(NWBAContext context)
        {
            const string format = "dd/MM/yyyy hh:mm:ss tt";
            context.Account.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = "S",
                    CustomerID = 2100,
                    Balance = 100,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = "C",
                    CustomerID = 2100,
                    Balance = 500,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = "S",
                    CustomerID = 2200,
                    Balance = 500.95m,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = "C",
                    CustomerID = 2300,
                    Balance = 1250.50m,
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            context.Database.OpenConnection();

            try
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Account ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Account OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }

        private static void AddTransactions(NWBAContext context)
        {
            const string format = "dd/MM/yyyy hh:mm:ss tt";
            const string openingBalance = "Opening balance";

            context.Transaction.AddRange(
                new Transaction
                {
                    TransactionType = "D",
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = "D",
                    AccountNumber = 4101,
                    Amount = 500,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = "D",
                    AccountNumber = 4200,
                    Amount = 500.95m,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = "D",
                    AccountNumber = 4300,
                    Amount = 1250.50m,
                    Comment = "Opening balance",
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            context.SaveChanges();
        }
    }
}
