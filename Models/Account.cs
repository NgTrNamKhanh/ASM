using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Required]
        [MinLength(10)]
        public string Usename { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Relationship
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Staff")]
        public int? StaffId { get; set; }
        public virtual Staff Staff { get; set; }

        [ForeignKey("Admin")]
        public int? AdminId { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
