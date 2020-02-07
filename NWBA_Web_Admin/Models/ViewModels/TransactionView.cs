using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Admin.Models.ViewModels
{
    public class TransactionView
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int? DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime ModifyDate { get; set; }

    }
}
