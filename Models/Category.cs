//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
	public class Category
	{
        [Required]
        [Key]
		public int CategoryId { get; set; }

		[Required, MaxLength(50)]
		public string CategoryName { get; set; }
		[Required]
		public string CategoryDescription { get; set; }

		//Relationship
		public List<CategoryProduct>? CategoryProducts { get; set; } 
	}
}
