using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA_Web_Application.Filters;
using NWBA_Web_Application.Models;
using NWBA_Web_Application.Utilities;
using X.PagedList;
using NWBA_Web_Application.Models.Business_Objects;

namespace NWBA_Web_Application.Controllers
{
    [AuthorisationFilter]
    [Route("[controller]")]
    public class StatementsController : Controller
    {
        private readonly AccountManager _acctRepo;
        private readonly TransactionManager _transRepo;
        public StatementsController(AccountManager acctRepo, TransactionManager transRepo)
        {
            _acctRepo = acctRepo;
            _transRepo = transRepo;
        }

        [Route("List")]
        public async Task<IActionResult> Index(StatementsViewModel model, int? page = 1)
        {
            ViewBag.Accounts = await this.GetAccountsForViewBag();

            const int pageSize = 4;
            if (model.id != 0)
            {
                HttpContext.Session.SetInt32("id", model.id);
            }

            if (model.id == 0 && HttpContext.Session.GetInt32("id").HasValue)
            {
                model.id = HttpContext.Session.GetInt32("id").Value;
            }

            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                var id = HttpContext.Session.GetInt32("id").Value;
                var pagedList = await _transRepo.GetTransactionPage(id, page, pageSize);
                model.Transactions = (IPagedList<Transaction>)pagedList;
            }
            else if(ViewBag.Accounts[0] != null)
            {
                var pagedList = await _transRepo.GetTransactionPage(ViewBag.Accounts[0].AccountNumber, page, pageSize);
                model.Transactions = (IPagedList<Transaction>)pagedList;
            }
            else
            {
                var pagedList = await _transRepo.GetTransactionPage(0, page, pageSize);
                model.Transactions = (IPagedList<Transaction>)pagedList;
            }

            return View(model);
        }

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