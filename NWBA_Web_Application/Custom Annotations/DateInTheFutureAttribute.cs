using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Custom_Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateInTheFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var futureDate = value as DateTime?;
            var memberNames = new List<string>() { context.MemberName };

            if (futureDate != null)
            {
                if (futureDate.Value.ToLocalTime() < DateTime.Now)
                {
                    return new ValidationResult("Date must be in the future.", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}
