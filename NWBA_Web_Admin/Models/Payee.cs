using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Admin.Models
{
    public partial class Payee
    {
        public Payee()
        {
            BillPay = new HashSet<BillPay>();
        }

        [Key]
        [Column("PayeeID")]
        public int PayeeId { get; set; }
        [Required]
        [StringLength(50)]
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

        [InverseProperty("Payee")]
        public virtual ICollection<BillPay> BillPay { get; set; }
    }
}
