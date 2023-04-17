namespace ASM.Models
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string CategoryDescription { get; set; }

		//Relationship
		public List<CategoryProduct> CategoryProducts { get; set; }
	}
}
