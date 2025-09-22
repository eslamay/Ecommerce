using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Ecommerce.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext dbContext;
		private readonly IMapper mapper;
		private readonly IImageManagementService imageManagementService;
		private readonly IConnectionMultiplexer redis;
		private readonly UserManager<AppUser> userManager;
		private readonly SignInManager<AppUser> signInManager;
		private readonly ISendEmail sendEmail;
		private readonly IGenerateToken token;


		public ICategoryRepository CategoryRepository { get; }

		public IProductRepository ProductRepository { get; }

		public IPhotoRepository PhotoRepository { get; }

		public ICustomerBasketRepository customerBasketRepository { get; }

		public IAuth auth { get; }

		public UnitOfWork(AppDbContext dbContext, IMapper mapper, IImageManagementService imageManagementService, IConnectionMultiplexer redis, UserManager<AppUser> userManager, ISendEmail sendEmail, SignInManager<AppUser> signInManager, IGenerateToken token)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			this.imageManagementService = imageManagementService;
			this.sendEmail = sendEmail;
			this.signInManager = signInManager;
			this.token = token;
			CategoryRepository = new CategoryRepository(dbContext);
			ProductRepository = new ProductRepository(dbContext, mapper, imageManagementService);
			PhotoRepository = new PhotoRepository(dbContext);
			customerBasketRepository = new CustomerBasketRepository(redis);
			this.redis = redis;
			this.userManager = userManager;
			auth = new AuthRepository(userManager, signInManager, sendEmail, token,dbContext);
		}
	}
}
