using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Filters;
using NWBA_Web_Application.Models;
using NWBA_Web_Application.Utilities;
using SimpleHashing;
using NWBA_Web_Application.Models.Business_Objects;

namespace NWBA_Web_Application.Controllers
{
    [AuthorisationFilter]
    public class CustomerController : Controller
    {
        private readonly CustomerManager _custRepo;
        private readonly AccountManager _acctRepo;
        private readonly LoginManager _loginRepo;

        public CustomerController(CustomerManager custRepo, AccountManager acctRepo, TransactionManager transRepo, LoginManager loginRepo, BillpayManager bpayRepo, PayeeManager payeeRepo)
        {
            _custRepo = custRepo;
            _acctRepo = acctRepo;
            _loginRepo = loginRepo;
        }

        public async Task<IActionResult> Index()
        {
            int customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            Customer customer = await _custRepo.Get(customerID);

            return View(customer);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id != HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value)
            {
                return NotFound();
            }

            Customer customer = await _custRepo.GetWithoutAccounts(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(string oldpass, string newpass, string newpass2)
        {
            var id = HttpContext.Session.GetInt32(nameof(Customer.CustomerID));
            var login = await _loginRepo.GetAlternative(id);

            if (!PBKDF2.Verify(login.PasswordHash, oldpass) || newpass != newpass2 || oldpass == newpass)
            {
                ModelState.AddModelError("ChangeFailed", "Couldn't reset password, please try again.");
                return View();
            }

            //converts to hash to put in the db
            string hash = PBKDF2.Hash(newpass);
            login.PasswordHash = hash;
            login.ModifyDate = DateTime.UtcNow;

            _loginRepo.Save();
            TempData["changed"] = "password";
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            ViewBag.Changed = TempData["changed"].ToString();
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id != HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value)
            {
                return NotFound();
            }

            var customer = await _custRepo.GetWithoutAccounts(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CustomerID,CustomerName,TFN,Address,City,State,PostCode,Phone")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _custRepo.Update(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_custRepo.CustomerExists(customer.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["changed"] = "details";
                return RedirectToAction("Success");
            }
            return View(customer);
        }

        // Helper methods
        private void CheckAmountError(decimal amount)
        {
            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        }

        private async Task<List<Account>> GetAccountsForViewBag()
        {
            var custID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
            var accounts = await _acctRepo.GetAccountsOfCustomer(custID);
            return (List<Account>)accounts;
        }
    }
}
