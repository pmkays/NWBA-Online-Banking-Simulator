using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        }
        
        [HttpPost]
        public async Task<IActionResult> Graphs(GraphViewModel formModel)
        {
            //convert time so that 
            //formModel.Date1 = DateTime.Parse(formModel.Date1.ToUniversalTime().ToString("dd/MM/yyyy"));
            //formModel.Date2 = DateTime.Parse(formModel.Date1.ToUniversalTime().ToString("dd/MM/yyyy"));

            string date1 = formModel.Date1.ToUniversalTime().ToString("dd-MM-yyyy");
            string date2 = formModel.Date1.ToUniversalTime().ToString("dd-MM-yyyy");
            HttpResponseMessage response; 
            if (formModel.CustomerID == 0)
            {
                //response = await WebApi.InitializeClient().GetAsync($"api/transactions/{formModel.Date1}/{formModel.Date2}");
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inrange?date1={date1}&date2={date2}");

            }
            else
            {
                int id = formModel.CustomerID;
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inrangewithid?id={id}&date1={date1}&date2={date2}");
            }

            if (!response.IsSuccessStatusCode || response is null)
            {
                throw new Exception();
            }

            var result = response.Content.ReadAsStringAsync().Result;
            var totals = JsonConvert.DeserializeObject<List<int>>(result);
            return RedirectToAction("DisplayGraphs", totals);
        }

        public IActionResult DisplayGraphs(List<int> totals)
        {
            //not done yet
            return View(totals);
        }

    }
}