﻿using NWBA_Web_Admin.Custom_Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Admin.Models
{
    public partial class Account
    {
        public Account()
        {
            BillPay = new HashSet<BillPay>();
            TransactionAccountNumberNavigation = new HashSet<Transaction>();
            TransactionDestinationAccountNumberNavigation = new HashSet<Transaction>();
        }

        [Key]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        [Required]
        [StringLength(1)]
        [AccountType]
        [Display(Name = "Type")]
        public string AccountType { get; set; }
        [Column("CustomerID")]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }
        [Column(TypeName = "money")]
        public decimal Balance { get; set; }
        public DateTime ModifyDate { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Account")]
        public virtual Customer Customer { get; set; }
        [InverseProperty("AccountNumberNavigation")]
        public virtual ICollection<BillPay> BillPay { get; set; }
        [InverseProperty(nameof(Transaction.AccountNumberNavigation))]
        public virtual ICollection<Transaction> TransactionAccountNumberNavigation { get; set; }
        [InverseProperty(nameof(Transaction.DestinationAccountNumberNavigation))]
        public virtual ICollection<Transaction> TransactionDestinationAccountNumberNavigation { get; set; }
    }
}
