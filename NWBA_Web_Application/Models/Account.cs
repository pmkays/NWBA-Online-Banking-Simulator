using NWBA_Web_Application.Custom_Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Application.Models
{
    public class Account
    {
        [Key]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required]
        [AccountType]
        [StringLength(1)]
        [Display(Name = "Type")]
        public string AccountType { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<BillPay> BillPays { get; set; }
    }

    public enum AccountType{
        Saving = 1,
        Checking = 2
    }
}