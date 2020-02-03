using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Models
{
    public class Payee
    {
        public int PayeeID { get; set;}
        [Required]
        [StringLength(50)]
        [Display(Name = "Payee Name")]
        public string PayeeName { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        public string State { get; set; }
        [StringLength(10)]
        public string PostCode { get; set; }
        [Required]
        [StringLength(15)]
        public string Phone { get; set; }
        public virtual List<BillPay> BillPays { get; set; }
    }


}