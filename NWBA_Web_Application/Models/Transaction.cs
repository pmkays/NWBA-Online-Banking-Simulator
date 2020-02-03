using NWBA_Web_Application.Custom_Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Application.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        [StringLength(1)]
        [TransactionType]
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        public virtual Account Account { get; set; }

        [Display(Name = "Destination Account")]
        public int? DestinationAccountNumber { get; set; }

        public virtual Account DestinationAccount { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime ModifyDate { get; set; }
    }

    public enum TransactionTypes
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
        ServiceCharge = 4,
        BillPay = 5
    }
}
