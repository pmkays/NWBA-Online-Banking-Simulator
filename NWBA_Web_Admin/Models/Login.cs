using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Web_Admin.Models
{
    public partial class Login
    {
        [Key]
        [Column("UserID")]
        [StringLength(50)]
        public string UserId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(64)]
        public string PasswordHash { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime BlockTime { get; set; }
        public int LoginAttempts { get; set; }
        [Required]
        public string Status { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Login")]
        public virtual Customer Customer { get; set; }
    }
}
