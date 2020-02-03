using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Models.Business_Objects
{
    public class PayeeManager : IDataRepository<Payee, int>
    {
        private readonly NWBAContext _context;

        public PayeeManager(NWBAContext context)
        {
            _context = context;
        }

        public async Task<Payee> Get(int id)
        {
            var payee = await _context.Payee.Where(x => x.PayeeID == id).FirstOrDefaultAsync();
            return payee;
        }

        public Payee GetUntracked(int id)
        {
            var payee = _context.Payee.Where(x => x.PayeeID == id).AsNoTracking().FirstOrDefault();
            return payee;
        }

        public async Task<IEnumerable<Payee>> GetAll()
        {
            var payee = await _context.Payee.ToListAsync();
            return payee;
        }

        public void Add(Payee payee)
        {
            _context.Add(payee);
            _context.SaveChanges();
        }

        public void Update(Payee payee)
        {
            _context.Update(payee);
            _context.SaveChanges();
        }
        public void Delete(Payee payee)
        {
            _context.Remove(payee);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
