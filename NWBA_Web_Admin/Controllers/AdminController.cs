using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction("Portal");
            }
            else
            {
                ModelState.AddModelError("LoginFailed", "Log in failed. Wrong username or password.");
            }

            return View();
        }

        [Route("Portal")]
        public ActionResult Portal()
        {
            if (!IsAdminLoggedOn())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminPresent");
            return RedirectToAction("Index","Home");
        }

        public Boolean IsAdminLoggedOn()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminPresent")))
            {
                return false;
            }
            return true;
        }

    }
}
