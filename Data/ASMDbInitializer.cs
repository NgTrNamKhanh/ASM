//using ASM.Migrations;
using ASM.Models;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using ASM.Constants;

namespace ASM.Data
{
    public class ASMDbInitializer
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
           


            using (var serivceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serivceScope.ServiceProvider.GetService<ASMContext>();

                context.Database.EnsureCreated();

                //Author
                if (!context.Author.Any())
                {
                    context.Author.AddRange(new List<Author>()
                    {
                        new Author()
                        {
                            AuthorName = "Haruki Murakami",
                            AuthorDescription ="Haruki Murakami (村上 春樹, Murakami Haruki, born January 12, 1949) is a Japanese writer. " +
                            "His novels, essays, and short stories have been bestsellers in Japan and internationally, with his work translated into 50 languages " +
                            "and having sold millions of copies outside Japan. He has received numerous awards for his work, including the Gunzo Prize for New Writers, " +
                            "the World Fantasy Award, the Frank O'Connor International Short Story Award, the Franz Kafka Prize, and the Jerusalem Prize.",
                            AuthorBirthYear = 1949
                        },
                        new Author()
                        {
                            AuthorName = "Milan Kundera",
                            AuthorDescription ="Milan Kundera (born 1 April 1929) is a Czech writer who went into exile in " +
                            "France in 1975, becoming a naturalised French citizen in 1981. Kundera's Czechoslovak citizenship was revoked in 1979, his Czech citizenship conferred in 2019. " +
                            "He \"sees himself as a French writer and insists his work should be studied as French literature and classified as such in book stores\"",
                            AuthorBirthYear = 1929
                        }
                    });
                    context.SaveChanges();
                }
                //Category
                if (!context.Category.Any())
                {
                    context.Category.AddRange(new List<Category>()
                    {
                        new Category()
                        {
                            CategoryName = "Romance novel",
                            CategoryDescription ="A romance novel or romantic novel generally refers to a type of genre fiction novel which places its primary " +
                            "focus on the relationship and romantic love between two people, and usually has an \"emotionally satisfying and optimistic ending." +
                            "\" Precursors include authors of literary fiction, such as Samuel Richardson, Jane Austen, and Charlotte Brontë."
                        },
                         new Category()
                        {
                            CategoryName = "Thriller novels",
                            CategoryDescription ="Thrillers are a genre of fiction in which tough, resourceful, but essentially ordinary heroes are pitted against " +
                            "villains determined to destroy them, their country, or the stability of the free world. Often associated with spy fiction, war fiction, " +
                            "adventure and detective fiction."
                        },
                         new Category()
                        {
                            CategoryName = "Philosophical fiction",
                            CategoryDescription ="Philosophical fiction refers to the class of works of fiction which devote a significant portion " +
                            "of their content to the sort of questions normally addressed in philosophy. These might explore any facet of the human condition, " +
                            "including the function and role of society, the nature and motivation of human acts, the purpose of life, ethics or morals, the role " +
                            "of art in human lives, the role of experience or reason in the development of knowledge, whether there exists free will, or any other " +
                            "topic of philosophical interest. Philosophical fiction works would include the so-called novel of ideas, including some science fiction," +
                            " utopian and dystopian fiction, and the Bildungsroman.."
                        },
                         new Category()
                        {
                            CategoryName = "Literary fiction",
                            CategoryDescription ="Literary fiction, mainstream fiction, non-genre fiction or serious fiction is a label that," +
                            " in the book trade, refers to market novels that do not fit neatly into an established genre (see genre fiction); or, " +
                            "otherwise, refers to novels that are character-driven rather than plot-driven, examine the human condition, use language in " +
                            "an experimental or poetic fashion, or are simply considered serious art."
                        },

                    });
                    context.SaveChanges();

                }
                //Product
                if (!context.Product.Any())
                {
                    context.Product.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            ProductName= "Norwegian Wood",
                            ProductDescription = "Norwegian Wood is a 1987 novel by Japanese author Haruki Murakami." +
                            " The novel is a nostalgic story of loss and burgeoning sexuality. It is told from the first-person perspective of Toru Watanabe, " +
                            "who looks back on his days as a college student living in Tokyo.Through Watanabe's reminiscences, readers see him develop relationships " +
                            "with two very different women—the beautiful yet emotionally troubled Naoko, and the outgoing, lively Midori.",
                            ProductImage = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1630460042i/11297.jpg",
                            ProductPrice = 14,
                            ProductStock = 100
                        },
                        new Product()
                        {
                            ProductName= "The Unbearable Lightness of Being",
                            ProductDescription = "The Unbearable Lightness of Being is a 1984 novel by Milan Kundera, about two women, two men, a dog, " +
                            "and their lives in the 1968 Prague Spring period of Czechoslovak history. Although written in 1982, the novel was not published until " +
                            "two years later, in a French translation. The original Czech text was published the following year. " +
                            "It was also translated to English from Czech by Michael Henry Heim and published in The New Yorker's March 19, 1984 issue under the \"Fiction\" " +
                            "section.",
                            ProductImage = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1265401884i/9717.jpg",
                            ProductPrice = 14,
                            ProductStock = 100
                        }
                    });
                    context.SaveChanges();
                    //AuthorProduct
                    if (!context.AuthorProduct.Any())
                    {
                        context.AuthorProduct.AddRange(new List<AuthorProduct>()
                        {
                            new AuthorProduct()
                            {
                                AuthorId = 1,
                                ProductId =1
                            },
                            new AuthorProduct()
                            {
                                AuthorId = 2,
                                ProductId =2
                            },

                        });
                        context.SaveChanges();

                    }
                    //CategoryProduct
                    if (!context.CategoryProduct.Any())
                    {
                        context.CategoryProduct.AddRange(new List<CategoryProduct>()
                        {
                            new CategoryProduct() 
                            {
                                CategoryId = 1,
                                ProductId =1
                            },
                            new CategoryProduct()
                            {
                                CategoryId = 4,
                                ProductId =1
                            },
                            new CategoryProduct()
                            {
                                CategoryId = 3,
                                ProductId =2
                            }
                        });
                        context.SaveChanges();
                    }
                    //Order Status 
                    if (!context.OrderStatus.Any())
                    {
                        context.OrderStatus.AddRange(new List<OrderStatus>()
                        {
                            new OrderStatus()
                            {
                                //Id = 1,
                                StatusId = 1,
                                StatusName = "Pending",
                            },
                            new OrderStatus()
                            {
                                //Id = 2,
                                StatusId = 2,
                                StatusName = "Shipped",
                            },
                            new OrderStatus()
                            {
                                //Id = 3,
                                StatusId = 3,
                                StatusName = "Delivered",
                            },
                            new OrderStatus()
                            {
                                //Id = 4,
                                StatusId = 4, 
                                StatusName = "Cancelled",
                            },
                            new OrderStatus()
                            {
                                //Id = 5,
                                StatusId = 5, 
                                StatusName = "Returned",
                            },
                            new OrderStatus()
                            {
                                //Id = 6,
                                StatusId = 6, 
                                StatusName = "Refund",
                            }
                        });
                        context.SaveChanges();
                    }



                }
            }
        }
    }
}
