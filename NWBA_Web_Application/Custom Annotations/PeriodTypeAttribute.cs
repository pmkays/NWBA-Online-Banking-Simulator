using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Custom_Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PeriodTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            List<string> periodTypes = new List<string>{ "s", "m", "q", "a"};
            var period = value as string;
            var memberNames = new List<string>() { context.MemberName };

            if (period != null)
            {
                if (!periodTypes.Contains(period.ToLower()))
                {
                    return new ValidationResult("Transaction type invalid.", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}
