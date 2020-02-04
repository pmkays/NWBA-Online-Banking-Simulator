using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;

namespace WebApi.Models.DataManagers
{
    public class BillPayManager : IDataRepository<BillPay, int>
    {
        public NWBAContext _context;

        public BillPayManager(NWBAContext context)
        {
            _context = context;
        }
        public int Add(BillPay item)
        {
            _context.BillPay.Add(item);
            _context.SaveChanges();
            return item.BillPayId;
        }

        public int Delete(int id)
        {
            _context.BillPay.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        public BillPay Get(int id)
        {
            return _context.BillPay.Where(x => x.BillPayId == id).FirstOrDefault();
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPay.ToList();
        }

        public int Update(int id, BillPay item)
        {
            _context.BillPay.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
