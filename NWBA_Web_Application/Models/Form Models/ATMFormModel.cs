using NWBA_Web_Application.Custom_Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Models
{
    public class ATMFormModel
    {
        [TransactionType]
        public string TransactionType { get; set; }
        [Required]
        public int AccountNumber { get; set; }
        public int? DestinationAccountNumber { get; set; }
        public decimal Balance { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}
