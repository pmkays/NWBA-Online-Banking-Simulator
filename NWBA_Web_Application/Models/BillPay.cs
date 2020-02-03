using NWBA_Web_Application.Custom_Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Application.Models
{
    public class BillPay
    {
        public int BillPayID { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        // The person we send it to
        [Required]
        [ValidPayee]
        public int PayeeID { get; set; }
        public virtual Payee Payee {get; set;}

        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0.00")]
        public decimal Amount { get; set; }

        [Required]
        [DateInTheFuture]
        [Display(Name = "Scheduled Date")]
        public DateTime ScheduleDate { get; set; }

        [Required]
        [StringLength(1)]
        [PeriodType]
        public string Period { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
    }

    public enum Period
    {
        Monthly = 1,
        Quarterly = 2,
        Annually = 3
    }
}
