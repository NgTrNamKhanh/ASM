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
        public int OrderStatus { get; set; }
        //Relationship
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
