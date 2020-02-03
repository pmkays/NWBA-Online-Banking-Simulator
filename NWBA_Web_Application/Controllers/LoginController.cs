using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA_Web_Application.Models;
using SimpleHashing;
using Microsoft.AspNetCore.Http;
using NWBA_Web_Application.Models.Business_Objects;

namespace NWBA_Web_Application.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginManager _loginRepo;
        public LoginController(LoginManager loginRepo)
        {
            _loginRepo = loginRepo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userID, string password)
        {
            var currLogin = await _loginRepo.GetFromUserID(userID);
            if (currLogin is null || !PBKDF2.Verify(currLogin.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View();
            }

            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), currLogin.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), currLogin.Customer.CustomerName);

            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(nameof(Customer.CustomerID));
            HttpContext.Session.Remove(nameof(Customer.CustomerName));
            HttpContext.Session.Remove("AdminPresent");
            return RedirectToAction("Index", "Home");
        }
    }
}
