using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ViewModels
{
    public class AmountDateCount
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public AmountDateCount(DateTime date, decimal amount)
        {
            Date = date;
            Amount = amount;
        }

    }
}
