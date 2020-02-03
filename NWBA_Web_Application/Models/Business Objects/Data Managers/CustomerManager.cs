using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Models.Business_Objects
{
    public class CustomerManager : IDataRepository<Customer, int>
    {
        private readonly NWBAContext _context;

        public CustomerManager(NWBAContext context)
        {
            _context = context;
        }

        public async Task<Customer> Get(int id)
        {
            var customer = await _context.Customer.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.CustomerID == id);
            return customer;

        }
        public async Task<Customer> GetWithoutAccounts(int? id)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(m => m.CustomerID == id);
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = await _context.Customer.Include("Account").ToListAsync();
            return customers;
        }

        public bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerID == id);
        }

        public void Add(Customer customer)
        {
            _context.Customer.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges(); 
        }

        public void Delete(Customer customer)
        {
            _context.Remove(customer);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
