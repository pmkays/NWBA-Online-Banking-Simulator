using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Admin.Models.ViewModels
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
