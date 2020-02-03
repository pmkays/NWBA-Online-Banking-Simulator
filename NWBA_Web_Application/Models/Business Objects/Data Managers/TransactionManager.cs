using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NWBA_Web_Application.Models.Business_Objects
{
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        private readonly NWBAContext _context;

        public TransactionManager(NWBAContext context)
        {
            _context = context;
        }

        public async Task<Transaction> Get(int id)
        {
            //get most recent transaction
            var transaction = await _context.Transaction.Where(x => x.AccountNumber == id).OrderByDescending(x => x.ModifyDate).FirstOrDefaultAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionPage(int id, int? page, int pageSize)
        {
            var pagedList = await _context.Transaction.Where(x => x.AccountNumber == id).OrderByDescending(x => x.ModifyDate).ToPagedListAsync(page, pageSize);
            return pagedList;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            var transactions = await _context.Transaction.ToListAsync();
            return transactions;
        }
     
        public void Add(Transaction transaction)
        {
            _context.Add(transaction);
            _context.SaveChanges();
        }

        public void Update(Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();
        }

        public void Delete(Transaction transaction)
        {
            _context.Remove(transaction);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
