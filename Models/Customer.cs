namespace ASM.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        //Relationship
        public ICollection<Order> Orders { get; set; }

        public virtual Account Account { get; set; }
    }
}
