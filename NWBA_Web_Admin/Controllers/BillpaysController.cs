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

namespace NWBA_Web_Admin.Controllers
{
    [Route("RestrictedBillpayAccess")]
    public class BillpaysController : Controller
    {

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

            if (billPay.Status == "Active")
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
    }
}