using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;

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

        public IEnumerable<TransDateCount> GetRangeAll(DateTime date1, DateTime date2)
        {
            //instantiate an array of new transaction date count objects
            List<TransDateCount> transDateCount = new List<TransDateCount>();

            //loops through the date range
            while (date1 != date2)
            {

                DateTime dayAfter = date1.AddDays(1);
                var amount = _context.Transaction.
                    Where(x => x.ModifyDate > date1 && x.ModifyDate < dayAfter).
                    GroupBy(x => x.ModifyDate.Date).
                    Select(x => x.Count()).
                    FirstOrDefault();

                //adds a new object where the date has been reconverted to local time instead of UTC

                DateTime localDate = date1.ToLocalTime();
                TransDateCount dateCount = new TransDateCount(localDate, amount);
                transDateCount.Add(dateCount);

                date1 = date1.AddDays(1);
            }
            return transDateCount;
        }


        public IEnumerable<TransDateCount> GetRange(int id, DateTime date1, DateTime date2)
        {
            //instantiate an array of new transaction date count objects
            List<TransDateCount> transDateCount = new List<TransDateCount>();

            //loops through the date range
            while (date1 != date2)
            {
                //must restrict the date to one day as a time; == doesn't work because it is datetime not date
                //must use theninclude because we are referencing the navigation property of account to get customer
                //group by statement converts the datetime to date
                DateTime dayAfter = date1.AddDays(1);
                var amount = _context.Transaction.
                    Include(x => x.AccountNumberNavigation).
                    ThenInclude(a => a.Customer).
                    Where(x => x.AccountNumberNavigation.CustomerId == id && x.ModifyDate > date1 && x.ModifyDate < dayAfter).
                    GroupBy(x => x.ModifyDate.Date).
                    Select(x => x.Count()).
                    FirstOrDefault();

                //adds a new object where the date has been reconverted to local time instead of UTC
                DateTime localDate = date1.ToLocalTime();
                TransDateCount dateCount = new TransDateCount(localDate, amount);
                transDateCount.Add(dateCount);
                date1 = date1.AddDays(1);
            }
            return transDateCount;
        }
    }
}
