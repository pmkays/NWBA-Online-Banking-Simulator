using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA_Web_Application.Models;
using NWBA_Web_Application.Utilities;
using X.PagedList;
using NWBA_Web_Application.Models.Business_Objects;
using NWBA_Web_Application.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace NWBA_Web_Application.Controllers
{
    [AuthorisationFilter]
    public class BillPayController : Controller
    {
        private readonly AccountManager _acctRepo;
        private readonly BillpayManager _bpayRepo;
        private readonly PayeeManager _payeeRepo;
        public BillPayController(AccountManager acctRepo, BillpayManager bpayRepo, PayeeManager payeeRepo)
        {
            _acctRepo = acctRepo;
            _bpayRepo = bpayRepo;
            _payeeRepo = payeeRepo;
        }

        public async Task<IActionResult> Index(BillPayViewModel model, int? page = 1)
        {
            ViewBag.Accounts = await this.GetAccountsForViewBag();
            const int pageSize = 4;

            if (model.id != 0)
            {
                HttpContext.Session.SetInt32("BillPayID", model.id);
            }

            if (model.id == 0 && HttpContext.Session.GetInt32("BillPayID").HasValue)
            {
                model.id = HttpContext.Session.GetInt32("BillPayID").Value;
            }

            if (HttpContext.Session.GetInt32("BillPayID").HasValue)
            {
                var sessionID = HttpContext.Session.GetInt32("BillPayID").Value;
                var pagedList = await _bpayRepo.GetTransactionPage(sessionID, page, pageSize);
                model.BillPays = (IPagedList<BillPay>)pagedList;
            }
            else if(ViewBag.Accounts[0] != null)
            {
                var pagedList = await _bpayRepo.GetTransactionPage(ViewBag.Accounts[0].AccountNumber, page, pageSize);
                model.BillPays = (IPagedList<BillPay>)pagedList;
            }
            else
            {
                var pagedList = await _bpayRepo.GetTransactionPage(0, page, pageSize);
                model.BillPays = (IPagedList<BillPay>)pagedList;
            }

            return View(model);
        }

        public async Task<IActionResult> ScheduleBillPay()
        {
            //populates view bags with accounts for dropboxes
            ViewBag.Accounts = await this.GetAccountsForViewBag();
            ViewBag.Payees = await _payeeRepo.GetAll();

            //creates a new form model and gets the current time with three minutes added
            BillPayFormModel formModel = new BillPayFormModel();
            formModel.Date = DateTime.Parse(DateTime.Now.AddMinutes(3).ToString("dd/MM/yyyy HH:mm"));
            return View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleBillPay(BillPayFormModel model)
        {
            Account account = await _acctRepo.GetAcctBpay(model.SenderAccountNumber);
            Payee payee = await _payeeRepo.Get(model.DestinationID);

            this.CheckAmountError(model.Amount);
            this.CanProceed(account, model.Amount);

            if (!ModelState.IsValid)
            {
                ViewBag.Payees = await _payeeRepo.GetAll();
                ViewBag.Accounts = await this.GetAccountsForViewBag();
                return View(model);
            }

            if (account == null || payee == null)
            {
                return NotFound();
            }

            string billpayJson = JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString("BillpayJson", billpayJson);

            return RedirectToAction(nameof(Confirm));
        }

        public IActionResult Confirm()
        {
            string billpayJson = HttpContext.Session.GetString("BillpayJson");
            if (billpayJson == null)
                return NotFound();

            BillPayFormModel model = JsonConvert.DeserializeObject<BillPayFormModel>(billpayJson);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(BillPayFormModel model)
        {
            Account account = await _acctRepo.GetAcctBpay(model.SenderAccountNumber);
            Payee payee = await _payeeRepo.Get(model.DestinationID);
            NWBASystem.GetInstance().SchedulePayment(account, payee, model.Amount, model.Date, model.Period);
            _acctRepo.Save();

            return RedirectToAction(nameof(SuccessfulSchedule));
        }

        public async Task<IActionResult> EditBillPay(int? id)
        {
            BillPay billPay = await _bpayRepo.GetBpay(id);
            if (id == null || billPay == null)
            {
                return NotFound();
            }
            var custID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var listOfCustomersBillPays = _bpayRepo.GetListOfBillPayIdFromCustomer(custID);
            if (!listOfCustomersBillPays.Contains((int)id))
            {
                return NotFound();
            }

            ViewBag.Accounts = await this.GetAccountsForViewBag();
            billPay.ScheduleDate = billPay.ScheduleDate.ToLocalTime();
            return View(billPay);
        }

        [HttpPost]
        public async Task<IActionResult> EditBillPay(int id, BillPay billPay)
        {
            if (id != billPay.BillPayID)
            {
                return NotFound();
            }

            var custID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var listOfCustomersBillPays = _bpayRepo.GetListOfBillPayIdFromCustomer(custID);
            if (!listOfCustomersBillPays.Contains(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    billPay.ScheduleDate = billPay.ScheduleDate.ToUniversalTime();
                    _bpayRepo.Update(billPay);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
                catch (DbUpdateException)
                {
                    return NotFound();
                }
                
                return View(nameof(SuccessfulEdit));
            }
            ViewBag.Accounts = await this.GetAccountsForViewBag();
            return View(billPay);
        }

        public IActionResult SuccessfulSchedule()
        {
            return View();
        }
        public IActionResult SuccessfulEdit()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            BillPay billPay = await _bpayRepo.GetBpayWithPayee(id);
            if (billPay == null)
                return NotFound();

            return View(billPay);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BillPay billPay)
        {
            BillPay billPayToDelete = await _bpayRepo.GetBpay(billPay.BillPayID);
            if (billPayToDelete == null)
                return NotFound();
            _bpayRepo.Delete(billPayToDelete);

            return RedirectToAction(nameof(SuccessfulEdit));
        }

        private void CheckAmountError(decimal amount)
        {
            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        }
        private void CanProceed(Account account, decimal amount)
        {
            //ensures business rules are followed
            decimal newBalance = account.Balance - amount;
            int minCheckingBalance = 200;

            if (account.AccountType == "C")
            {
                if (newBalance < minCheckingBalance)
                {
                    ModelState.AddModelError(nameof(amount), "Insufficient funds; Checking account must have at least $200 remaining after the transaction.");
                }
            }
            else
            {
                if (newBalance < 0)
                {
                    ModelState.AddModelError(nameof(amount), "Insufficient funds.");
                }
            }
        }

        private async Task<List<Account>> GetAccountsForViewBag()
        {
            var custID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var accounts = await _acctRepo.GetAccountsOfCustomer(custID);
            return (List<Account>)accounts;
        }
    }
}