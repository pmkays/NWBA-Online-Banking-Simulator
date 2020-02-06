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

            var customers = await this.GetCustomers();
            ViewBag.AllCustomers = customers;
            return View(formModel);
        }
        
        //[HttpPost]
        //public async Task<IActionResult> Graphs(GraphViewModel formModel)
        //{

        //    var customers = await this.GetCustomers();
        //    ViewBag.AllCustomers = customers;

        //    string date1 = formModel.Date1.ToUniversalTime().ToString("dd-MM-yyyy hh:mm:ss");
        //    string date2 = formModel.Date2.ToUniversalTime().ToString("dd-MM-yyyy hh:mm:ss");

        //    HttpResponseMessage response; 

        //    if (formModel.CustomerID == 0)
        //    {
        //        response = await WebApi.InitializeClient().GetAsync($"api/transactions/inrange?date1={date1}&date2={date2}");
        //    }
        //    else
        //    {
        //        int id = formModel.CustomerID;
        //        response = await WebApi.InitializeClient().GetAsync($"api/transactions/inrangewithid?id={id}&date1={date1}&date2={date2}");
        //    }

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception();
        //        //return View(formModel); 
        //    }

        //    //gets the results of the transdatecount objs as strings
        //    var result = response.Content.ReadAsStringAsync().Result;

        //    //just checking if the deserializing works which it does
        //    //var transactions = JsonConvert.DeserializeObject<List<TransDateCount>>(result);


        //    //return to the view where the graph will be rendered
        //    ViewBag.GraphData = result;
        //    ViewBag.GraphType = formModel.GraphType;

        //    return View();
        //}

        [HttpPost]
        public async Task<IEnumerable<TransDateCount>> Graphs(GraphViewModel formModel)
        {
            //the model is meant to be in json format when passed through so we convert it back
            //var formModel = JsonConvert.DeserializeObject<GraphViewModel>(modelString);

            var customers = await this.GetCustomers();
            ViewBag.AllCustomers = customers;

            string date1 = formModel.Date1.ToUniversalTime().ToString("dd-MM-yyyy hh:mm:ss");
            string date2 = formModel.Date2.ToUniversalTime().ToString("dd-MM-yyyy hh:mm:ss");

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

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
                //return View(formModel); 
            }

            //gets the results of the transdatecount objs as JsonString
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            //converts it to objects again 
            var transPDay = JsonConvert.DeserializeObject<List<TransDateCount>>(result);


            //so we can choose what type of graph to display
            ViewBag.GraphType = formModel.GraphType;

            return transPDay;
        }
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


    }
}