using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public partial class BillPay
    {
        [Key]
        [Column("BillPayID")]
        public int BillPayId { get; set; }
        public int AccountNumber { get; set; }
        [Column("PayeeID")]
        public int PayeeId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public DateTime ScheduleDate { get; set; }
        [Required]
        [StringLength(1)]
        public string Period { get; set; }
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
