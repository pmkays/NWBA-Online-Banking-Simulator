using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Models.Business_Objects
{
    public class AccountManager : IDataRepository<Account, int>
    {
        private readonly NWBAContext _context;

        public AccountManager(NWBAContext context)
        {
            _context = context;
        }

        public async Task<Account> Get(int id)
        {
            var account =  await _context.Account.Include(x => x.Transactions).FirstOrDefaultAsync(x => x.AccountNumber == id);
            return account;
        }

        public async Task<Account> GetDest(int? id)
        {
            var account = await _context.Account.Include(x => x.Transactions).FirstOrDefaultAsync(x => x.AccountNumber == id);
            return account;
        }

        public async Task<Account> GetAcctBpay(int id)
        {
            var account = await _context.Account.Include(x => x.BillPays).Where(x => x.AccountNumber == id).FirstOrDefaultAsync();
            return account;
        }

        public async Task<IEnumerable<Account>> GetAccountsOfCustomer(int id)
        {
            var accounts  = await _context.Account.Include("Customer").Where(x => x.CustomerID == id).ToListAsync();
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            var accounts = await _context.Account.Include("Customer").ToListAsync();
            return accounts;
        }

        public void Add(Account account)
        {
            _context.Add(account);
            _context.SaveChanges(); 
        }

        public void Update(Account account)
        {
            _context.Update(account);
            _context.SaveChanges();
        }

        public void Delete(Account account)
        {
            _context.Remove(account);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
