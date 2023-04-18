namespace ASM.Models
{
	public class Product
	{
		public Product() 
		{
			AuthorProducts = new List<AuthorProduct>();
			CategoryProducts = new List<CategoryProduct>();
			OrderDetails = new List<OrderDetail>();
		}
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductDescription { get; set; }
		public float ProductPrice { get; set; }
		public int ProductStock { get; set; }
		public string ProductImage { get; set; }
		//Relationship
		public List<AuthorProduct> AuthorProducts { get; set; }

		public List<CategoryProduct> CategoryProducts { get; set; }

		public List<OrderDetail> OrderDetails { get; set; }
	}
}
