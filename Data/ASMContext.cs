using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ASM.Data
{
    public class ASMContext : IdentityDbContext
    {
        public ASMContext (DbContextOptions<ASMContext> options)
            : base(options)
        {
        }

		public DbSet<ASM.Models.Product> Product { get; set; } = default!;
		public DbSet<ASM.Models.Author> Author { get; set; } = default!;
		public DbSet<ASM.Models.Category> Category { get; set; } = default!;
		public DbSet<ASM.Models.AuthorProduct> AuthorProduct { get; set; } = default!;
		public DbSet<ASM.Models.CategoryProduct> CategoryProduct { get; set; } = default!;

		public DbSet<ASM.Models.Order> Order { get; set; } = default!;
		public DbSet<ASM.Models.OrderDetail> OrderDetail { get; set; } = default!;

		public DbSet<ASM.Models.Account> Account { get; set; } = default!;
		public DbSet<ASM.Models.Customer> Customer { get; set; } = default!;
		public DbSet<ASM.Models.Staff> Staff { get; set; } = default!;
		public DbSet<ASM.Models.Admin> Admin { get; set; } = default!;


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			//AuthorProduct
			modelBuilder.Entity<AuthorProduct>().HasKey(ap => new
			{
				ap.AuthorId,
				ap.ProductId
			});
			modelBuilder.Entity<AuthorProduct>().HasOne(p => p.Product).WithMany(ap => ap.AuthorProducts).HasForeignKey(p => p.ProductId);

			modelBuilder.Entity<AuthorProduct>().HasOne(p => p.Author).WithMany(ap => ap.AuthorProducts).HasForeignKey(p => p.AuthorId);

			base.OnModelCreating(modelBuilder);
			//Category Product
			modelBuilder.Entity<CategoryProduct>().HasKey(cp => new
			{
				cp.CategoryId,
				cp.ProductId
			});
			modelBuilder.Entity<CategoryProduct>().HasOne(p => p.Product).WithMany(cp => cp.CategoryProducts).HasForeignKey(p => p.ProductId);

			modelBuilder.Entity<CategoryProduct>().HasOne(p => p.Category).WithMany(cp => cp.CategoryProducts).HasForeignKey(p => p.CategoryId);

			base.OnModelCreating(modelBuilder);


			//Order Customer

			modelBuilder.Entity<Customer>()
				.HasMany(c => c.Orders)
				.WithOne(o => o.Customer)
				.HasForeignKey(o => o.CustomerId);

			//OrderDetail - Order-Product
			modelBuilder.Entity<OrderDetail>().HasKey(cp => new
			{
				cp.OrderId,
				cp.ProductId
			});
			modelBuilder.Entity<OrderDetail>().HasOne(p => p.Product).WithMany(cp => cp.OrderDetails).HasForeignKey(p => p.ProductId);

			modelBuilder.Entity<OrderDetail>().HasOne(p => p.Order).WithMany(cp => cp.OrderDetails).HasForeignKey(p => p.OrderId);

			base.OnModelCreating(modelBuilder);

			//Account to Customer
			modelBuilder.Entity<Account>()
			.HasOne(a => a.Customer)
			.WithOne(c => c.Account)
			.HasForeignKey<Customer>(c => c.CustomerId);
			//Account to Staff
			modelBuilder.Entity<Account>()
				.HasOne(a => a.Staff)
				.WithOne(s => s.Account)
				.HasForeignKey<Staff>(s => s.StaffId);
			//Account to Admin
			modelBuilder.Entity<Account>()
				.HasOne(a => a.Admin)
				.WithOne(ad => ad.Account)
				.HasForeignKey<Admin>(ad => ad.AdminId);
		}
	}
}
