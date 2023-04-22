using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public string CustomerID { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<CartDetails> CartDetails { get; set; }
    }
}
