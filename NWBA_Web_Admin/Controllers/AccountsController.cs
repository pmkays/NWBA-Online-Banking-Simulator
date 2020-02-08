using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWBA_Web_Admin.Models;

namespace NWBA_Web_Admin.Controllers
{
    [Route("RestrictedAccountAccess")]
    public class AccountsController : Controller
    {
        [HttpGet("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var response = await WebApi.InitializeClient().GetAsync($"api/accounts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var account = JsonConvert.DeserializeObject<Account>(result);

            return View(account);
        }

        [HttpPost("DeleteAccountSuccess/{id}")]
        public IActionResult DeleteAccountSuccess(int id)
        {
            var response = WebApi.InitializeClient().DeleteAsync($"api/Accounts/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return RedirectToAction("ViewCustomers");
        }
    }
}