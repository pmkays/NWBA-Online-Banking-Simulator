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

        public IEnumerable<int> GetRangeAll(DateTime date1, DateTime date2)
        {
            return _context.Transaction.Where(x => x.ModifyDate >= date1 && x.ModifyDate <= date2).
                OrderByDescending(x => x.ModifyDate).GroupBy(x=> x.ModifyDate).Select(x => x.Count()).ToList();
        }

        public IEnumerable<int> GetRange(int id, DateTime date1, DateTime date2)
        {
            return _context.Transaction.Where(x => x.ModifyDate >= date1 && x.ModifyDate <= date2 && x.AccountNumberNavigation.CustomerId == id).
                OrderByDescending(x => x.ModifyDate).GroupBy(x => x.ModifyDate).Select(x => x.Count()).ToList();
        }
    }
}
