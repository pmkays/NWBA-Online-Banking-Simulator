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
    [Route("RestrictedAdminAccess")]
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
