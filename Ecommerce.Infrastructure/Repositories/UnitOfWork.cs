using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext dbContext;

		public ICategoryRepository CategoryRepository { get; }

		public IProductRepository ProductRepository { get; }

		public IPhotoRepository PhotoRepository { get; }

		public UnitOfWork(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
			CategoryRepository = new CategoryRepository(dbContext);
			ProductRepository = new ProductRepository(dbContext);
			PhotoRepository = new PhotoRepository(dbContext);
		}
	}
}
