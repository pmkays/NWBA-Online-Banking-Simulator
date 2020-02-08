using NWBA_Web_Admin.Custom_Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Admin.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Account = new HashSet<Account>();
        }

        [Key]
        [Column("CustomerID")]
        [Display(Name = "ID")]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        [Column("TFN")]
        [StringLength(11)]
        [Display(Name = "Tax File No.")]
        public string Tfn { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        [StringLength(20)]
        [State]
        public string State { get; set; }
        [StringLength(4)]
        public string PostCode { get; set; }
        [StringLength(14)]
        [RegularExpression(@"^\(61\)-\s[0-9]{8}$", ErrorMessage = "Phone Number should be in this format: (61)- xxxxxx")]
        public string Phone { get; set; }

        [InverseProperty("Customer")]
        public virtual Login Login { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Account> Account { get; set; }
    }
}
