using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    [Route("RestrictedTransactionAccess")]
    public class TransactionsController : Controller
    {
        [HttpGet("Graphs")]
        public async Task<IActionResult> Graphs(int? id)
        {
            GraphViewModel formModel = new GraphViewModel();
            if(id != null)
            {
                formModel.CustomerID = (int)id;
            }
            formModel.Date1 = DateTime.Parse("19/12/2019");
            formModel.Date2 = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

            var customers = await this.GetCustomers();
            ViewBag.AllCustomers = customers;
            return View(formModel);
        }

        [HttpPost("BarGraph")]
        public async Task<IEnumerable<TransDateCount>> BarGraph(GraphViewModel formModel)
        {

            string date1 = formModel.Date1.ToString("dd-MM-yyyy hh:mm:ss");
            string date2 = formModel.Date2.ToString("dd-MM-yyyy hh:mm:ss");

            HttpResponseMessage response;

            if (formModel.CustomerID == 0)
            {
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inbar?date1={date1}&date2={date2}");
            }
            else
            {
                int id = formModel.CustomerID;
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inbarwithid?id={id}&date1={date1}&date2={date2}");
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<TransDateCount>();
            }

            //gets the results of the transdatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;

            //converts it to objects again 
            var transPDay = JsonConvert.DeserializeObject<List<TransDateCount>>(result);

            foreach (TransDateCount trans in transPDay)
            {
                trans.Date = DateTime.Parse(trans.Date.ToString("dd/MM/yy"));
            }

            return transPDay;
        }

        [HttpPost("Tables")]
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

            if (!response.IsSuccessStatusCode)
            {
                return new List<TransactionView>();
            }

            //gets the results of the transdatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;

            //converts it to objects again 
            var transactions = JsonConvert.DeserializeObject<List<TransactionView>>(result);
            return transactions;
        }

        [HttpPost("PieGraph")]
        public async Task<IEnumerable<TransTypeDateCount>> PieGraph(GraphViewModel formModel)
        {

            string date1 = formModel.Date1.ToString("dd-MM-yyyy hh:mm:ss");
            string date2 = formModel.Date2.ToString("dd-MM-yyyy hh:mm:ss");

            HttpResponseMessage response;

            if (formModel.CustomerID == 0)
            {
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inpie?date1={date1}&date2={date2}");
            }
            else
            {
                int id = formModel.CustomerID;
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inpiewithid?id={id}&date1={date1}&date2={date2}");
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<TransTypeDateCount>();
            }

            //gets the results of the transtypedatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;

            //converts it to objects again 
            var transTypes = JsonConvert.DeserializeObject<List<TransTypeDateCount>>(result);

            return transTypes;
        }

        [HttpPost("LineGraph")]
        public async Task<IEnumerable<AmountDateCount>> LineGraph(GraphViewModel formModel)
        {

            string date1 = formModel.Date1.ToString("dd-MM-yyyy hh:mm:ss");
            string date2 = formModel.Date2.ToString("dd-MM-yyyy hh:mm:ss");

            HttpResponseMessage response;

            if (formModel.CustomerID == 0)
            {
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inline?date1={date1}&date2={date2}");
            }
            else
            {
                int id = formModel.CustomerID;
                response = await WebApi.InitializeClient().GetAsync($"api/transactions/inlinewithid?id={id}&date1={date1}&date2={date2}");
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<AmountDateCount>();
            }

            //gets the results of the amountdatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;

            //converts it to objects again 
            var amountDates = JsonConvert.DeserializeObject<List<AmountDateCount>>(result);

            foreach (AmountDateCount amountDate in amountDates)
            {
                amountDate.Date = DateTime.Parse(amountDate.Date.ToString("dd/MM/yy"));
            }

            return amountDates;
        }

        //helper methods
        private async Task<List<Customer>> GetCustomers()
        {
            var response = await WebApi.InitializeClient().GetAsync("api/customers");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Customer>();
            }

            var result = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Customer>>(result);
        }

        private void CheckDates(DateTime date1, DateTime date2)
        {
            if(date2 < date1 || date2 > DateTime.Now || date1 > DateTime.Now)
            {
                ModelState.AddModelError(nameof(date2), "Please ensure dates are valid");
            }
        }
    }
}