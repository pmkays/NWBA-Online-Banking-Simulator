using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWBA_Web_Admin.Models;
using NWBA_Web_Admin.Models.ViewModels;

namespace NWBA_Web_Admin.Controllers
{
    public class TransactionsController : Controller
    {
        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var response = await WebApi.InitializeClient().GetAsync("api/transactions");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);

            return View(transactions);
        }

        public async Task<IActionResult> Graphs()
        {
            GraphViewModel formModel = new GraphViewModel();
            formModel.Date1 = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            formModel.Date2 = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

            var response = await WebApi.InitializeClient().GetAsync("api/customers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = response.Content.ReadAsStringAsync().Result;

            var customers = JsonConvert.DeserializeObject<List<Customer>>(result);
            ViewBag.AllCustomers = customers;
            return View(formModel);
            //var response = await WebApi.InitializeClient().GetAsync("api/transactions");

            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new Exception();
            //}

            //// Storing the response details recieved from web api.
            //var result = response.Content.ReadAsStringAsync().Result;

            //// Deserializing the response recieved from web api and storing into a list.
            //var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);

            //return View(transactions);
        }

    }
}