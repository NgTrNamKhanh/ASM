using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
    public class CartDetails
    {
        public int id { get; set; }
        [Required]
        public int CartID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]

        public Product Product { get; set; }
        public Cart Cart { get; set; }
    }
}
