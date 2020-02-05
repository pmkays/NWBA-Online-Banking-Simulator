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
        public int Add(Account item)
        {
            _context.Account.Add(item);
            _context.SaveChanges();
            return item.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Account.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        public Account Get(int id)
        {
            return _context.Account.Include(x => x.TransactionAccountNumberNavigation).FirstOrDefault(x => x.AccountNumber == id);
        }

        public IEnumerable<Account> GetAccountFromCustomer(int id)
        {
            return _context.Account.Include(x => x.TransactionAccountNumberNavigation).Where(x => x.CustomerId == id).ToList();
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Account.Include(x => x.TransactionAccountNumberNavigation).ToList();
        }

        public int Update(int id, Account item)
        {
            _context.Account.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
