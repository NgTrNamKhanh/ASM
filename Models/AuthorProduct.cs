namespace ASM.Models
{
	public class AuthorProduct
	{
		public int AuthorId { get; set; }
		public Author Author { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
	}
}
