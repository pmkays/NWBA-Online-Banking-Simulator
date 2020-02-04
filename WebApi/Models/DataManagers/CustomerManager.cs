using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;

namespace WebApi.Models.DataManagers
{
    public class CustomerManager : IDataRepository<Customer, int>
    { 
        public NWBAContext _context;

        public CustomerManager (NWBAContext context)
        {
            _context = context;
        }
        public int Add(Customer item)
        {
            _context.Customer.Add(item);
            _context.SaveChanges();
            return item.CustomerId;
        }

        public int Delete(int id)
        {
            _context.Customer.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        public Customer Get(int id)
        {
           return _context.Customer.Include(x => x.Account).FirstOrDefault(x => x.CustomerId == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customer.Include(x => x.Account).ToList();
        }

        public int Update(int id, Customer item)
        {
            _context.Customer.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
