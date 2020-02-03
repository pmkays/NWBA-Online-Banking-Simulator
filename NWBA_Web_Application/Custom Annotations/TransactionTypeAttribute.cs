using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Custom_Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TransactionTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            List<string> transactionTypes = new List<string>{ "s", "b", "d", "t", "w" };
            var transactionType = value as string;
            var memberNames = new List<string>() { context.MemberName };

            if (transactionType != null)
            {
                if (!transactionTypes.Contains(transactionType.ToLower()))
                {
                    return new ValidationResult("Transaction type invalid.", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}
