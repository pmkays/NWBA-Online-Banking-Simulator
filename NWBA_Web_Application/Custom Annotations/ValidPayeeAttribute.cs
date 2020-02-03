using NWBA_Web_Application.Data;
using NWBA_Web_Application.Models.Business_Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Custom_Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidPayeeAttribute : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var payeeID = value as int?;
            var memberNames = new List<string>() { context.MemberName };
            var service = (PayeeManager)context.GetService(typeof(PayeeManager));

            if (payeeID != null)
            {
                var payee = service.GetUntracked((int)payeeID);
                if (payee == null)
                {
                    return new ValidationResult("Payee ID invalid.", memberNames);

                }
            }

            return ValidationResult.Success;
        }
    }
}
