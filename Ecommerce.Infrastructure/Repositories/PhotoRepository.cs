using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repositories
{
	public class PhotoRepository : GenricRepository<Photo>, IPhotoRepository
	{
		public PhotoRepository(AppDbContext dbContext) : base(dbContext)
		{
		}
	}
}
