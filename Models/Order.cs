using Microsoft.Build.Framework;
using Microsoft.VisualBasic;

namespace ASM.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusID { get; set; }
        public float OrderTotalPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        public string CustomerId { get; set; }
        //Relationship
       
        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}