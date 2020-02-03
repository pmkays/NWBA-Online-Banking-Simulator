
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NWBA_Web_Application.Data;
using NWBA_Web_Application.Models;
using NWBA_Web_Application.Models.Business_Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.BackgroundService
{
    public class ScopedBillPayTimeKeeper : BillPayTimeKeeper
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedBillPayTimeKeeper(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider);
            }
        }

        public async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Current Time: " + DateTime.Now);
            var bpayRepo = serviceProvider.GetService<BillpayManager>();
            var accRepo = serviceProvider.GetService<AccountManager>();
            BillPay billPay = await bpayRepo.GetEarliestBillPay();

            if (billPay != null)
            {
                //Debugging Code - Ignore writelines
                Console.WriteLine("------\nDate looking for " + billPay.ScheduleDate.ToLocalTime() + "\nTime Difference: " + (billPay.ScheduleDate.ToLocalTime() - DateTime.Now) +
                    "\nTime Equal: " + AreEqual(DateTime.Now, billPay.ScheduleDate.ToLocalTime()) + "\n------");


                if (AreEqual(DateTime.Now, billPay.ScheduleDate.ToLocalTime()))
                {
                    Account account = await accRepo.Get(billPay.AccountNumber);
                    if (CanProceed(account, billPay.Amount))
                    {
                        NWBASystem.GetInstance().PayBill(account, billPay.Amount, billPay.Payee);
                        UpdateNextScheduledDate(billPay);
                        if (DeleteBillPayIfNeeded(billPay))
                        {
                            bpayRepo.Delete(billPay);
                        }
                        else
                        {
                            bpayRepo.Update(billPay);
                        }
                    }
                    else
                    {
                        bpayRepo.Delete(billPay);
                    }

                    Console.WriteLine("Scheduled payment has been made!");
                }
            };

        }
        private void UpdateNextScheduledDate(BillPay billPay)
        {
            if (billPay.Period == "M")
            {
                billPay.ScheduleDate = billPay.ScheduleDate.AddMonths(1);
            }
            if (billPay.Period == "Q")
            {
                billPay.ScheduleDate = billPay.ScheduleDate.AddMonths(3);
            }
            if (billPay.Period == "A")
            {
                billPay.ScheduleDate = billPay.ScheduleDate.AddMonths(12);
            }
        }

        private Boolean CanProceed(Account account, decimal amount)
        {
            //ensures business rules are followed
            decimal newBalance = account.Balance - amount;
            int minCheckingBalance = 200;

            if (account.AccountType == "C")
            {
                if (newBalance < minCheckingBalance)
                {
                    return false;
                }
            }
            else
            {
                if (newBalance < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private Boolean DeleteBillPayIfNeeded(BillPay billPay)
        {
            if (billPay.Period == "S")
            {
                return true;
            }
            else return false;
        }
    }
}


