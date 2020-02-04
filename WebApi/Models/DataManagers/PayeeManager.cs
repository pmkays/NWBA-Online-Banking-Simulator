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
        public int Add(Payee item)
        {
            _context.Payee.Add(item);
            _context.SaveChanges();
            return item.PayeeId;
        }

        public int Delete(int id)
        {
            _context.Payee.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        public Payee Get(int id)
        {
            return _context.Payee.Include(x => x.BillPay).FirstOrDefault(x => x.PayeeId == id);
        }

        public IEnumerable<Payee> GetAll()
        {
            return _context.Payee.Include(x => x.BillPay).ToList();
        }

        public int Update(int id, Payee item)
        {
            _context.Payee.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
