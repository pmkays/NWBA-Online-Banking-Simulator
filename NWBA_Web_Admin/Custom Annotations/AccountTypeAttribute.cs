using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Admin.Custom_Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AccountTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            List<string> accountTypes = new List<string> { "s", "c" };
            var accountType = value as string;
            var memberNames = new List<string>() { context.MemberName };

            if (accountType != null)
            {
                if (!accountTypes.Contains(accountType.ToLower()))
                {
                    return new ValidationResult("Account type invalid.", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}
