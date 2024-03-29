﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Admin.Models
{
    public partial class BillPay
    {
        [Key]
        [Column("BillPayID")]
        [Display(Name = "ID")]
        public int BillPayId { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        [Column("PayeeID")]
        [Display(Name = "Payee ID")]
        public int PayeeId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Display(Name = "Scheduled Date")]
        public DateTime ScheduleDate { get; set; }
        [Required]
        [StringLength(1)]
        public string Period { get; set; }
        [Display(Name = "Modify Date")]
        public DateTime ModifyDate { get; set; }
        [Required]
        public string Status { get; set; }

        [ForeignKey(nameof(AccountNumber))]
        [InverseProperty(nameof(Account.BillPay))]
        public virtual Account AccountNumberNavigation { get; set; }
        [ForeignKey(nameof(PayeeId))]
        [InverseProperty("BillPay")]
        public virtual Payee Payee { get; set; }
    }
}
