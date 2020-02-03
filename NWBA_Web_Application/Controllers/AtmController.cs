using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWBA_Web_Application.Filters;
using NWBA_Web_Application.Models;
using NWBA_Web_Application.Models.Business_Objects;
using NWBA_Web_Application.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Controllers
{
    [AuthorisationFilter]
    public class AtmController : Controller
    {

        private readonly AccountManager _acctRepo;
        public AtmController(AccountManager acctRepo)
        {

            _acctRepo = acctRepo;

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Accounts = await this.GetAccountsForViewBag();
            var allAccounts = await _acctRepo.GetAll();
            ViewBag.AllAccounts = allAccounts;
            return View(new ATMFormModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ATMFormModel model)
        {
            //methods to retrieve have to be different because int? vs int
            var account = await _acctRepo.Get(model.AccountNumber);
            var destAccount = await _acctRepo.GetDest(model.DestinationAccountNumber);

            var amount = model.Amount;
            string comment = model.Comment;

            if (string.IsNullOrEmpty(comment))
            {
                comment = " ";
            }

            if (model.TransactionType == "T" && (destAccount == null || destAccount == account))
            {
                ModelState.AddModelError(nameof(model.DestinationAccountNumber), "Please ensure destination account number is valid");
            }

            this.CheckAmountError(amount);

            if (model.TransactionType != "D")
            {
                this.CanProceed(model.TransactionType, account, amount);
            }

            if (!ModelState.IsValid)
            {
                return await this.Index();
            }

            string customerJson = JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString("CustomerJson", customerJson);

            return RedirectToAction(nameof(Confirm));
        }

        public IActionResult Confirm()
        {
            string customerJson = HttpContext.Session.GetString("CustomerJson");
            if (customerJson == null)
                return NotFound();

            ATMFormModel model = JsonConvert.DeserializeObject<ATMFormModel>(customerJson);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Confirm(ATMFormModel model)
        {
            //methods to retrieve have to be different because int? vs int
            var account = await _acctRepo.Get(model.AccountNumber);
            var destAccount = await _acctRepo.GetDest(model.DestinationAccountNumber);

            var amount = model.Amount;
            string comment = model.Comment;

            switch (model.TransactionType)
            {
                case ("W"):
                    NWBASystem.GetInstance().Withdraw(account, amount);
                    break;
                case ("D"):
                    NWBASystem.GetInstance().Deposit(account, amount);
                    break;
                case ("T"):
                    NWBASystem.GetInstance().Transfer(account, destAccount, amount, comment);
                    break;
            }
            _acctRepo.Save();
            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }
        private void CheckAmountError(decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            }

            if (amount.HasMoreThanTwoDecimalPlaces())
            {
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            }
        }
        private void CanProceed(string type, Account account, decimal amount)
        {
            int numberOfTransactions = account.Transactions.Count;
            int maxTransactions = 4;
            decimal newBalance = 0;
            decimal wService = (decimal)0.1;
            decimal tService = (decimal)0.2;
            int minCheckingBalance = 200; 

            if (numberOfTransactions < maxTransactions)
            {
                newBalance = account.Balance - amount;
            }
            else
            {
                if (type == "W")
                {
                    //include service charge for withdraw
                    newBalance = account.Balance - amount - wService;
                }
                else
                {
                    //include service charge for transfer
                    newBalance = account.Balance - amount - tService;
                }
            }

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