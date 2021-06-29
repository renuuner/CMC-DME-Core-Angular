using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMC_DME_Core_Angular.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string AccountName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string ContactName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string ContactNumber { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool Status { get; set; }
    }
}
