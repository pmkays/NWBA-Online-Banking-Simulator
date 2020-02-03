using System;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Web_Application.Models
{
    public class Login
    {
        [Key, StringLength(50)]
        public string UserID { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }
        [Required]
        public int LoginAttempts { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime BlockTime { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
    }
}