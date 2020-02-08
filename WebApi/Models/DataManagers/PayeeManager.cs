using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;

namespace WebApi.Models.DataManagers
{
    public class PayeeManager : IDataRepository<Payee, int>
    {
        public NWBAContext _context;

        public PayeeManager(NWBAContext context)
        {
            _context = context;
        }

        //adds a payee
        public int Add(Payee item)
        {
            _context.Payee.Add(item);
            _context.SaveChanges();
            return item.PayeeId;
        }

        //deletes a payee
        public int Delete(int id)
        {
            _context.Payee.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        //gets a specific payee
        public Payee Get(int id)
        {
            return _context.Payee.Include(x => x.BillPay).FirstOrDefault(x => x.PayeeId == id);
        }

        //gets all payees
        public IEnumerable<Payee> GetAll()
        {
            return _context.Payee.Include(x => x.BillPay).ToList();
        }

        //deletes a specific payee
        public int Update(int id, Payee item)
        {
            _context.Payee.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
