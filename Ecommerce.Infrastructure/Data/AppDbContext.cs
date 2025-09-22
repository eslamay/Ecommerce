using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Entities.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecommerce.Infrastructure.Data
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{
		}

		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Photo> Photos { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Address> Addresses { get; set; }
		public virtual DbSet<Orders> Orders { get; set; }
		public virtual DbSet<OrderItem> OrderItems { get; set; }
		public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(assembly:Assembly.GetExecutingAssembly());
		}
	}
}
