﻿using Microsoft.EntityFrameworkCore;
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
        public int Add(Transaction item)
        {
            _context.Transaction.Add(item);
            _context.SaveChanges();
            return item.TransactionId;
        }

        public int Delete(int id)
        {
            _context.Transaction.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        public Transaction Get(int id)
        {
            return _context.Transaction.Where(x => x.AccountNumber == id).OrderByDescending(x => x.ModifyDate).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transaction.OrderByDescending(x => x.ModifyDate).ToList();
        }

        public int Update(int id, Transaction item)
        {
            _context.Transaction.Update(item);
            _context.SaveChanges();
            return id;
        }

        public IEnumerable<AmountDateCount> GetLineAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            //instantiate an array of new amount date count objects
            List<AmountDateCount> amountDateCount = new List<AmountDateCount>();

            //loops through the date range
            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time; == doesn't work because it is datetime not date
                //group by statement converts the datetime to date
                //add the amount
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

        public IEnumerable<AmountDateCount> GetLine(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            //instantiate an array of new amount date count objects
            List<AmountDateCount> amountDateCount = new List<AmountDateCount>();

            //loops through the date range
            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time; == doesn't work because it is datetime not date
                //must use theninclude because we are referencing the navigation property of account to get customer
                //group by statement converts the datetime to date
                //add the amount
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

        public IEnumerable<TransDateCount> GetRangeAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();
            //instantiate an array of new transaction date count objects
            List<TransDateCount> transDateCount = new List<TransDateCount>();

            //loops through the date range
            while (date1 != date2.AddDays(1))
            {

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


        public IEnumerable<TransDateCount> GetRange(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime();

            //instantiate an array of new transaction date count objects
            List<TransDateCount> transDateCount = new List<TransDateCount>();

            //loops through the date range
            while (date1 != date2.AddDays(1))
            {
                //must restrict the date to one day as a time; == doesn't work because it is datetime not date
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

        public IEnumerable<TransactionView> GetTableAll(DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime().AddDays(1);

            var transactions = _context.Transaction.
                Where(x => x.ModifyDate >= date1 && x.ModifyDate <= date2).
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


        public IEnumerable<TransactionView> GetTable(int id, DateTime date1, DateTime date2)
        {
            date1 = date1.ToUniversalTime();
            date2 = date2.ToUniversalTime().AddDays(1);
;
            var transactions = _context.Transaction.
                Include(x => x.AccountNumberNavigation).
                ThenInclude(a => a.Customer).
                Where(x => x.AccountNumberNavigation.CustomerId == id && x.ModifyDate >= date1 && x.ModifyDate <= date2).
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
