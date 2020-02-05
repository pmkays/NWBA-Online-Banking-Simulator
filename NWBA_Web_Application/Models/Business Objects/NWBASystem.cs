using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Models
{
    public class NWBASystem
    {
        // Singleton
        private static readonly NWBASystem Instance = new NWBASystem();

        private const int freeTransactionLimit = 4;
        private NWBASystem()
        {

        }

        public static NWBASystem GetInstance()
        {
            return Instance;
        }

        public void Withdraw(Account account, decimal amount)
        {
            int numberOfTransactions = account.Transactions.Count;

            decimal surcharge = (decimal)0.1;

            if (numberOfTransactions > freeTransactionLimit)
            {
                account.Balance -= (amount + surcharge);
                AddTransaction(account, surcharge, "S");
            }
            else
            {
                account.Balance -= amount;
            }
            account.ModifyDate = DateTime.UtcNow;
            AddTransaction(account, amount, "W");
        }

        public void Deposit(Account account, decimal amount)
        {
            account.Balance += amount;
            account.ModifyDate = DateTime.UtcNow;
            AddTransaction(account, amount, "D");
        }

        public void Transfer(Account account, Account destAcct, decimal amount, string comment)
        {

            decimal surcharge = (decimal)0.2;
            int numberOfTransactions = account.Transactions.Count;
            if (numberOfTransactions > freeTransactionLimit)
            {
                account.Balance -= (amount + surcharge);
                AddTransaction(account, surcharge, "S");
            }
            else
            {
                account.Balance -= amount;
            }

            destAcct.Balance += amount;
            account.ModifyDate = DateTime.UtcNow;
            AddTransaction(account, amount, "T", comment, destAcct.AccountNumber);
        }

        public void PayBill(Account account, decimal amount, Payee payee)
        {
            string comment = $"Bill pay to {payee.PayeeName} {payee.PayeeID}";
            account.Balance -= amount;
            account.ModifyDate = DateTime.UtcNow;
            AddTransaction(account, amount, "B", comment);
        }

        //used for bill pay transactions
        private void AddTransaction(Account account, decimal amount, string type, string comment)
        {
            List<Transaction> transactions = account.Transactions;
            transactions.Add(new Transaction
            {
                TransactionType = type,
                AccountNumber = account.AccountNumber,
                Amount = amount,
                Comment = comment,
                ModifyDate = DateTime.UtcNow
            });
        }

        //used for service charge, withdraw and deposit as it has no comments because it is an automated transaction
        private void AddTransaction(Account account, decimal amount, string type)
        {
            List<Transaction> transactions = account.Transactions;
            transactions.Add(new Transaction
            {
                TransactionType = type,
                AccountNumber = account.AccountNumber,
                Amount = amount,
                ModifyDate = DateTime.UtcNow
            });
        }

        //used for transfers as it has extra destination account parameter
        private void AddTransaction(Account account, decimal amount, string type, string comment, int destAcct)
        {
            List<Transaction> transactions = account.Transactions;
            transactions.Add(new Transaction
            {
                TransactionType = type,
                AccountNumber = account.AccountNumber,
                DestinationAccountNumber = destAcct,
                Amount = amount,
                Comment = comment,
                ModifyDate = DateTime.UtcNow
            });;
        }

        public void SchedulePayment(Account account, Payee payee, decimal amount, DateTime date, string period)
        {
            BillPay newBillPay = new BillPay {
                AccountNumber = account.AccountNumber,
                PayeeID = payee.PayeeID,
                Amount = amount,
                ScheduleDate = date.ToUniversalTime(),
                Period = period,
                ModifyDate = DateTime.UtcNow,
                Status = "Active"
            };

            account.BillPays.Add(newBillPay);
        }
    }
}


