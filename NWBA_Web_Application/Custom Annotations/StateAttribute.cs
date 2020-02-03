using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Custom_Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            List<string> states = new List<string>{ "act", "vic", "qld", "nsw", "sa","tas","wa","nt"};
            var state = value as string;
            var memberNames = new List<string>() { context.MemberName };

            if (state != null)
            {
                if (!states.Contains(state.ToLower()))
                {
                    return new ValidationResult("State invalid.", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}
