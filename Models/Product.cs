using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
    public class Product
    {
        //public Product() 
        //{
        //	AuthorProducts = new List<AuthorProduct>();
        //	CategoryProducts = new List<CategoryProduct>();
        //	OrderDetails = new List<OrderDetail>();
        //          SelectedCategoryIds = new List<int>();
        //          SelectedAuthorIds = new List<int>();
        //	Categories = new List<Category>();
        //	Authors = new List<Author>();	


        //      }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public string ProductImage { get; set; }
        //Relationship
        public List<AuthorProduct> AuthorProducts { get; set; } = new List<AuthorProduct>();

        public List<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();


        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public List<CartDetails> CartDetails { get; set; }


        [NotMapped]
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> SelectedAuthorIds { get; set; } = new List<int>();
        [NotMapped]
        public Category Category { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        [NotMapped]
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}