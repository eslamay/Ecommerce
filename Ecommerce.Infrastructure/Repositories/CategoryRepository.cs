using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repositories
{
	public class CategoryRepository : GenricRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(AppDbContext dbContext) : base(dbContext)
		{
		}
	}
}
