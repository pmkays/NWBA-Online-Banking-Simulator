using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWBA_Web_Admin.Models;
using NWBA_Web_Admin.Models.ViewModels;

namespace NWBA_Web_Admin.Controllers
{
    [Route("SuperSecretLogin")]
    public class AdminController : Controller
    {
        // GET: Admin
        [Route("Login")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public ActionResult Index(string userID, string password)
        {

            if (userID == "admin" && password == "admin")
            {
                HttpContext.Session.SetInt32("AdminPresent", 1);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("LoginFailed", "Log in failed. Wrong username or password.");
            }

            return View();
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminPresent");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("ViewCustomers")]
        public async Task<IActionResult> ViewCustomers()
        {
            var response = await WebApi.InitializeClient().GetAsync("api/customers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(result);

            return View(customers);
        }

        [HttpGet("CustomerDetails/{id}")]
        public async Task<IActionResult> CustomerDetails(int id)
        {

            HttpContext.Session.SetInt32("CurrentCustomer", id);

            var response = await WebApi.InitializeClient().GetAsync($"api/customers/{id}");
            var accounts = await WebApi.InitializeClient().GetAsync($"api/Accounts/AccountFromCustomer/{id}");

            if (!response.IsSuccessStatusCode || !accounts.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;
            var result2 = accounts.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var customer = JsonConvert.DeserializeObject<Customer>(result);
            var accountsList = JsonConvert.DeserializeObject<List<Account>>(result2);

            CustomerDetailsViewModel model = new CustomerDetailsViewModel
            {
                Customer = customer,
                Accounts = accountsList
            };

            return View(model);
        }

        [HttpGet("EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var response = await WebApi.InitializeClient().GetAsync($"api/customers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }

        [HttpPost("EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(int id, Customer customer)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = WebApi.InitializeClient().PutAsync($"api/Customers/{id}", content).Result;

                //Console.WriteLine(JsonConvert.SerializeObject(customer));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("ViewCustomers");
            }

            return View(customer);
        }

        [HttpGet("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var response = await WebApi.InitializeClient().GetAsync($"api/customers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }

        [HttpPost("DeleteCustomerSuccess/{id}")]
        public IActionResult DeleteCustomerSuccess(int id)
        {
            var response = WebApi.InitializeClient().DeleteAsync($"api/Customers/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return RedirectToAction("ViewCustomers");

        }

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

        [HttpGet("ScheduledBillPays/{id}")]
        public async Task<IActionResult> ScheduledBillPays(int id)
        {
            var response = await WebApi.InitializeClient().GetAsync($"api/BillPays/BillPaysFromAccount/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var billPays = JsonConvert.DeserializeObject<List<BillPay>>(result);

            ViewBag.AccountID = id;
            ViewBag.CustomerID = HttpContext.Session.GetInt32("CurrentCustomer");

            return View(billPays);
        }

        [HttpGet("DeleteBillPay/{id}")]
        public async Task<IActionResult> DeleteBillPay(int id)
        {
            var response = await WebApi.InitializeClient().GetAsync($"api/billpays/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var billPay = JsonConvert.DeserializeObject<BillPay>(result);

            return View(billPay);
        }

        [HttpPost("DeleteBillPaySuccess/{id}")]
        public IActionResult DeleteBillPaySuccess(int id)
        {
            var response = WebApi.InitializeClient().DeleteAsync($"api/billpays/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return RedirectToAction("ViewCustomers");

        }

        [HttpGet("StatusUpdate/{id}")]
        public async Task<IActionResult> UnblockBlock(int id)
        {
            var response = await WebApi.InitializeClient().GetAsync($"api/BillPays/{id}");

            if (response == null)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var billPay = JsonConvert.DeserializeObject<BillPay>(result);

            if(billPay.Status == "Active")
            {
                billPay.Status = "Blocked";
            }
            else
            {
                billPay.Status = "Active";
            }

            var content = new StringContent(JsonConvert.SerializeObject(billPay), Encoding.UTF8, "application/json");
            Console.WriteLine(JsonConvert.SerializeObject(billPay));
            var update = WebApi.InitializeClient().PutAsync($"api/BillPays/{id}", content).Result;

            if (!update.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return RedirectToAction("ScheduledBillPays", new { id = billPay.AccountNumber });

        }

        public bool IsAdminLoggedOn()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminPresent")))
            {
                return false;
            }
            return true;
        }

    }
}
