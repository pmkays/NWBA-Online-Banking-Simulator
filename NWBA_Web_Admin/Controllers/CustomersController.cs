using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWBA_Web_Admin.Filters;
using NWBA_Web_Admin.Models;
using NWBA_Web_Admin.Models.ViewModels;

namespace NWBA_Web_Admin.Controllers
{
    [AuthorisationFilter]
    [Route("RestrictedCustomerAccess")]
    public class CustomersController : Controller
    {

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
        public IActionResult EditCustomer(int id, Customer customer)
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
    }
}