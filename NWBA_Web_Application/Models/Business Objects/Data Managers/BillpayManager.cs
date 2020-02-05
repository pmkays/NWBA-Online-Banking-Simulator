using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NWBA_Web_Application.Models.Business_Objects
{
    public class BillpayManager : IDataRepository<BillPay, int>
    {
        private readonly NWBAContext _context;

        public BillpayManager(NWBAContext context)
        {
            _context = context;
        }

        public async Task<BillPay> Get(int id)
        {
            var bpay = await _context.BillPay.Where(x => x.BillPayID == id).FirstOrDefaultAsync();
            return bpay;
        }

        public async Task<BillPay> GetBpay(int? id)
        {
            var bpay = await _context.BillPay.Where(x => x.BillPayID == id).FirstOrDefaultAsync();
            return bpay;
        }

        public async Task<BillPay> GetBpayWithPayee(int? id)
        {
            var bpay = await _context.BillPay.Include(x=>x.Payee).Where(x => x.BillPayID == id).FirstOrDefaultAsync();
            return bpay;
        }

        public async Task<BillPay> GetEarliestBillPay()
        {
            string status = "Active";
            return await _context.BillPay.Include(x => x.Payee).OrderBy(x => x.ScheduleDate).Where(x=>x.Status== status).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BillPay>> GetAll()
        {
            var bpay = await _context.BillPay.Include(x=>x.Payee).ToListAsync();
            return bpay;
        }

        public async Task<IEnumerable<BillPay>> GetTransactionPage(int id, int? page, int pageSize)
        {
            var pagedList = await _context.BillPay.Include(x=>x.Payee).Where(x => x.AccountNumber == id).OrderByDescending(x => x.ModifyDate).ToPagedListAsync(page, pageSize);
            return pagedList;
        }

        public List<int> GetListOfBillPayIdFromCustomer(int custId)
        {
            var customer = _context.Customer.Include(x => x.Accounts).Where(x => x.CustomerID == custId).AsNoTracking().FirstOrDefault();
            List<int> accountNumbers = new List<int>();
            if (customer.Accounts != null)
            {
                foreach (Account account in customer.Accounts)
                {
                    accountNumbers.Add(account.AccountNumber);
                }
            }

            List<BillPay> billPays = new List<BillPay>();
            List<int> billPayIdList = new List<int>();
            if (accountNumbers != null)
            {
                foreach (int accountNumber in accountNumbers)
                {
                    billPays.AddRange(_context.BillPay.Where(x => x.AccountNumber == accountNumber).AsNoTracking().ToList());

                }
            }

            if (billPays != null)
            {
                foreach (BillPay billPay in billPays)
                {
                    billPayIdList.Add(billPay.BillPayID);
                }
            }

            return billPayIdList;
        }

        public void Add(BillPay bpay)
        {
            _context.Add(bpay);
            _context.SaveChanges();
        }

        public void Update(BillPay bpay)
        {
            _context.Update(bpay);
            _context.SaveChanges();
        }

        public void Delete(BillPay bpay)
        {
            _context.Remove(bpay);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
