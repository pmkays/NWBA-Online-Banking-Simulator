using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Admin.Filters
{
    //credit to Matthew Bolger
    public class AuthorisationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var adminPresent = context.HttpContext.Session.GetInt32("AdminPresent").HasValue;
            if (!adminPresent)
            {
                context.Result = new RedirectToActionResult("Index", "Admin", null);
            }
        }
    }

}
