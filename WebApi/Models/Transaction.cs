using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public partial class Transaction
    {
        [Key]
        [Column("TransactionID")]
        public int TransactionId { get; set; }
        [Required]
        [StringLength(1)]
        public string TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int? DestinationAccountNumber { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public DateTime ModifyDate { get; set; }

        [ForeignKey(nameof(AccountNumber))]
        [InverseProperty(nameof(Account.TransactionAccountNumberNavigation))]
        public virtual Account AccountNumberNavigation { get; set; }
        [ForeignKey(nameof(DestinationAccountNumber))]
        [InverseProperty(nameof(Account.TransactionDestinationAccountNumberNavigation))]
        public virtual Account DestinationAccountNumberNavigation { get; set; }
    }
}
