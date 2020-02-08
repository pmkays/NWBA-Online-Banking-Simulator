using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;

namespace WebApi.Models.DataManagers
{
    public class AccountManager : IDataRepository<Account, int>
    {
        public NWBAContext _context;

        public AccountManager(NWBAContext context)
        {
            _context = context;
        }

        //adds an account
        public int Add(Account item)
        {
            _context.Account.Add(item);
            _context.SaveChanges();
            return item.AccountNumber;
        }

        //deletes account
        public int Delete(int id)
        {
            _context.Account.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        //gets specific account
        public Account Get(int id)
        {
            return _context.Account.Include(x => x.TransactionAccountNumberNavigation).FirstOrDefault(x => x.AccountNumber == id);
        }

        //gets the accounts of a customer from customer id
        public IEnumerable<Account> GetAccountFromCustomer(int id)
        {
            return _context.Account.Include(x => x.TransactionAccountNumberNavigation).Where(x => x.CustomerId == id).ToList();
        }

        //gets all accounts of customers
        public IEnumerable<Account> GetAll()
        {
            return _context.Account.Include(x => x.TransactionAccountNumberNavigation).ToList();
        }

        //updates an account
        public int Update(int id, Account item)
        {
            _context.Account.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
