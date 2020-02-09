using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;
using WebApi.Models.ViewModels;

namespace WebApi.Models.DataManagers
{
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        public NWBAContext _context;

        public TransactionManager(NWBAContext context)
        {
            _context = context;
        }

        //adds a transaction
        public int Add(Transaction item)
        {
            _context.Transaction.Add(item);
            _context.SaveChanges();
            return item.TransactionId;
        }

        //deletes a transaction
        public int Delete(int id)
        {
            _context.Transaction.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        //gets a transaction
        public Transaction Get(int id)
        {
            return _context.Transaction.Where(x => x.AccountNumber == id).OrderByDescending(x => x.ModifyDate).FirstOrDefault();
        }

        //gets all transactions
        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transaction.OrderByDescending(x => x.ModifyDate).ToList();
        }

        //updates a transaction
        public int Update(int id, Transaction item)
        {
            _context.Transaction.Update(item);
            _context.SaveChanges();
            return id;
        }

        //gets line graph data for all users
        public IEnumerable<AmountDateCount> GetLineAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            List<AmountDateCount> amountDateCount = new List<AmountDateCount>();

            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time as == doesn't work with DateTime objects
                //group by statement converts the datetime to date
                //add the amounts together
                DateTime dayAfter = date1.AddDays(1);
                var amount = _context.Transaction.
                    Include(x => x.AccountNumberNavigation).
                    ThenInclude(a => a.Customer).
                    Where(x => x.ModifyDate >= date1 && x.ModifyDate <= dayAfter && x.TransactionType == "D").
                    GroupBy(x => x.ModifyDate.Date).
                    Select(x => x.Sum(y => y.Amount)).
                    FirstOrDefault();

                //adds a new object where the date has been reconverted to local time instead of UTC
                DateTime localDate = date1.ToLocalTime();
                AmountDateCount dateCount = new AmountDateCount(localDate, amount);
                amountDateCount.Add(dateCount);
                date1 = dayAfter; 
            }
            return amountDateCount;
        }

        //gets line graph data for a specific user
        public IEnumerable<AmountDateCount> GetLine(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            List<AmountDateCount> amountDateCount = new List<AmountDateCount>();

            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time as == doesn't work with DateTime objects
                //must use theninclude because we are referencing the navigation property of account to get customer
                //group by statement converts the datetime to date
                //add the amounts together
                DateTime dayAfter = date1.AddDays(1);
                var amount = _context.Transaction.
                    Include(x => x.AccountNumberNavigation).
                    ThenInclude(a => a.Customer).
                    Where(x => x.AccountNumberNavigation.CustomerId == id && x.ModifyDate >= date1 && x.ModifyDate <= dayAfter && x.TransactionType == "D").
                    GroupBy(x => x.ModifyDate.Date).
                    Select(x => x.Sum(y=> y.Amount)).
                    FirstOrDefault();

                //adds a new object where the date has been reconverted to local time instead of UTC
                DateTime localDate = date1.ToLocalTime();
                AmountDateCount dateCount = new AmountDateCount(localDate, amount);
                amountDateCount.Add(dateCount);
                date1 = dayAfter; 
            }
            return amountDateCount;
        }

        //gets pie graph data for all customers
        public IEnumerable<TransTypeDateCount> GetPieAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            var transactionTypes = _context.Transaction.
                Where(x => x.ModifyDate >= date1 && x.ModifyDate <= date2.AddDays(1)).
                GroupBy(x => x.TransactionType).
                Select(x => new TransTypeDateCount
                {
                    Type = x.Key,
                    Count = x.Count()
                }).
                ToList();

            return transactionTypes; 
        }

        //gets pie graph data for a specific customer
        public IEnumerable<TransTypeDateCount> GetPie(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            var transactionTypes = _context.Transaction.
                Include(x => x.AccountNumberNavigation).
                ThenInclude(a => a.Customer).
                Where(x => x.AccountNumberNavigation.CustomerId == id && x.ModifyDate >= date1 && x.ModifyDate <= date2.AddDays(1)).
                GroupBy(x => x.TransactionType).
                Select(x => new TransTypeDateCount
                {
                    Type = x.Key,
                    Count = x.Count()
                }).
                ToList();

            return transactionTypes;
        }

        //gets bar graph data for all customers
        public IEnumerable<TransDateCount> GetBarAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            List<TransDateCount> transDateCount = new List<TransDateCount>();

            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time as == doesn't work with DateTime objects
                //group by statement converts the datetime to date
                DateTime dayAfter = date1.AddDays(1);
                var amount = _context.Transaction.
                    Where(x => x.ModifyDate >= date1 && x.ModifyDate <= dayAfter).
                    GroupBy(x => x.ModifyDate.Date).
                    Select(x => x.Count()).
                    FirstOrDefault();

                //adds a new object where the date has been reconverted to local time instead of UTC
                DateTime localDate = date1.ToLocalTime();
                TransDateCount dateCount = new TransDateCount(localDate, amount);
                transDateCount.Add(dateCount);

                date1 = dayAfter;
            }
            return transDateCount;
        }

        //gets bar graph data for a specific customer
        public IEnumerable<TransDateCount> GetBar(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            List<TransDateCount> transDateCount = new List<TransDateCount>();

            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time as == doesn't work with DateTime objects
                //must use theninclude because we are referencing the navigation property of account to get customer
                //group by statement converts the datetime to date
                DateTime dayAfter = date1.AddDays(1);
                var amount = _context.Transaction.
                    Include(x => x.AccountNumberNavigation).
                    ThenInclude(a => a.Customer).
                    Where(x => x.AccountNumberNavigation.CustomerId == id && x.ModifyDate >= date1 && x.ModifyDate <= dayAfter).
                    GroupBy(x => x.ModifyDate.Date).
                    Select(x => x.Count()).
                    FirstOrDefault();

                //adds a new object where the date has been reconverted to local time instead of UTC
                DateTime localDate = date1.ToLocalTime();
                TransDateCount dateCount = new TransDateCount(localDate, amount);
                transDateCount.Add(dateCount);
                date1 = dayAfter; 
            }
            return transDateCount;
        }

        //gets table for all customers
        public IEnumerable<TransactionView> GetTableAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime().AddDays(1);

            var transactions = _context.Transaction.
                Where(x => x.ModifyDate >= date1 && x.ModifyDate <= date2).OrderByDescending(x => x.ModifyDate).
                Select(x => new TransactionView
                {
                    TransactionId = x.TransactionId,
                    TransactionType = x.TransactionType,
                    AccountNumber = x.AccountNumber,
                    DestinationAccountNumber = x.DestinationAccountNumber,
                    Amount = x.Amount,
                    Comment = x.Comment,
                    ModifyDate = x.ModifyDate.ToLocalTime()
                }).
                ToList();
            return transactions; 
        }

        //gets table for a specific customer
        public IEnumerable<TransactionView> GetTable(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime().AddDays(1);

            //must use theninclude because we are referencing the navigation property of account to get customer
            //group by statement converts the datetime to date
            var transactions = _context.Transaction.
                Include(x => x.AccountNumberNavigation).
                ThenInclude(a => a.Customer).
                Where(x => x.AccountNumberNavigation.CustomerId == id && x.ModifyDate >= date1 && x.ModifyDate <= date2).OrderByDescending(x => x.ModifyDate).
                Select(x => new TransactionView
                { 
                    TransactionId = x.TransactionId, 
                    TransactionType = x.TransactionType,
                    AccountNumber = x.AccountNumber, 
                    DestinationAccountNumber = x.DestinationAccountNumber, 
                    Amount = x.Amount,
                    Comment = x.Comment, 
                    ModifyDate = x.ModifyDate.ToLocalTime()
                }).
                ToList();
            return transactions;
        }
    }
}
