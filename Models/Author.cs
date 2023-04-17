namespace ASM.Models
{
	public class Author
	{
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
		public string AuthorDescription { get; set; }
		public int AuthorBirthYear { get; set; }
		//Relationship
		public List<AuthorProduct> AuthorProducts { get; set; }
	}
}
