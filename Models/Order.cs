using Microsoft.VisualBasic;

namespace ASM.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public float OrderTotalPrice { get; set; }
        public int OrderStatus { get; set; }
        //Relationship
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

    }
}
