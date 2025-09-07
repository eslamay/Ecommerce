using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repositories
{
	public class ProductRepository : GenricRepository<Product>, IProductRepository
	{
		public ProductRepository(AppDbContext dbContext) : base(dbContext)
		{
		}
	}
}
