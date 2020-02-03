using NWBA_Web_Application.Custom_Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        [StringLength(11)]
        [Display(Name = "Tax File Number")]
        public string TFN { get; set; }

        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        [StringLength(20)]
        [State]
        public string State { get; set; }
        [StringLength(10)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [StringLength(14)]
        [RegularExpression(@"^\(61\)-\s[0-9]{8}$", ErrorMessage = "Phone Number should be in this format: (61)- xxxxxx")]
        public string Phone { get; set; }
        public virtual Login Login { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
}