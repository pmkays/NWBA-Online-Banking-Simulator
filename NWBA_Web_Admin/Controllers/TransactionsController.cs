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
            formModel.Date1 = DateTime.Parse("19/12/2019");
            formModel.Date2 = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

            var customers = await this.GetCustomers();
            ViewBag.AllCustomers = customers;
            return View(formModel);
        }

        [HttpPost]
        public async Task<IEnumerable<TransDateCount>> Graphs(GraphViewModel formModel)
        {

            string date1 = formModel.Date1.ToString("dd-MM-yyyy hh:mm:ss");
            string date2 = formModel.Date2.ToString("dd-MM-yyyy hh:mm:ss");

            HttpResponseMessage response;

            if (formModel.CustomerID == 0)
            {
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inrange?date1={date1}&date2={date2}");
            }
            else
            {
                int id = formModel.CustomerID;
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inrangewithid?id={id}&date1={date1}&date2={date2}");
            }

            this.CheckDates(formModel.Date1, formModel.Date2);

            if (!response.IsSuccessStatusCode || !ModelState.IsValid)
            {
                //what to return because you can't return the view
            }

            //gets the results of the transdatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;

            //converts it to objects again 
            var transPDay = JsonConvert.DeserializeObject<List<TransDateCount>>(result);

            return transPDay;
        }

        public IActionResult Tables()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IEnumerable<TransactionView>> Tables(GraphViewModel formModel)
        {

            string date1 = formModel.Date1.ToString("dd-MM-yyyy hh:mm:ss");
            string date2 = formModel.Date2.ToString("dd-MM-yyyy hh:mm:ss");

            HttpResponseMessage response;

            if (formModel.CustomerID == 0)
            {
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/intable?date1={date1}&date2={date2}");
            }
            else
            {
                int id = formModel.CustomerID;
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/intablewithid?id={id}&date1={date1}&date2={date2}");
            }

            this.CheckDates(formModel.Date1, formModel.Date2);

            if (!response.IsSuccessStatusCode || !ModelState.IsValid)
            {
               //nani
            }

            //gets the results of the transdatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;

            //converts it to objects again 
            var transactions = JsonConvert.DeserializeObject<List<TransactionView>>(result);
            return transactions;
        }



        //helper methods
        private async Task<List<Customer>> GetCustomers()
        {
            var response = await WebApi.InitializeClient().GetAsync("api/customers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var result = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Customer>>(result);
        }

        private void CheckDates(DateTime date1, DateTime date2)
        {
            if(date2 < date1 || (date2.Date - date1.Date).TotalDays > 30 || date2 > DateTime.Now)
            {
                ModelState.AddModelError(nameof(date2), "Please ensure dates are valid");
            }

        }


    }
}