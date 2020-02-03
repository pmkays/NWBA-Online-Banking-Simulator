using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWBA_Web_Application.Models;

namespace NWBA_Web_Application.Filters
{
    //credit to Matthew Bolger
    public class AuthorisationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var custID = context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).HasValue;
            if (!custID)
            {
                //this is why it redirects to index method of home controller
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
    }
}
