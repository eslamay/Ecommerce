using AutoMapper;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;

namespace Ecommerce.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext dbContext;
		private readonly IMapper mapper;
		private readonly IImageManagementService imageManagementService;

		public ICategoryRepository CategoryRepository { get; }

		public IProductRepository ProductRepository { get; }

		public IPhotoRepository PhotoRepository { get; }

		public UnitOfWork(AppDbContext dbContext, IMapper mapper, IImageManagementService imageManagementService)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			this.imageManagementService = imageManagementService;
			CategoryRepository = new CategoryRepository(dbContext);
			ProductRepository = new ProductRepository(dbContext, mapper, imageManagementService);
			PhotoRepository = new PhotoRepository(dbContext);
		}
	}
}
