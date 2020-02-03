using NWBA_Web_Application.Custom_Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Models
{
    public class BillPayFormModel
    {
        [Required]
        public int SenderAccountNumber { get; set; }
        [Required]
        [ValidPayee]
        public int DestinationID { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0.00")]
        public decimal Amount { get; set; }
        [Required]
        [DateInTheFuture]
        public DateTime Date { get; set; }
        [Required]
        [PeriodType]
        public string Period { get; set; }
    }
}
