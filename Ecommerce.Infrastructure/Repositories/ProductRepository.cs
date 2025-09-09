using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Core.Sharing;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ecommerce.Infrastructure.Repositories
{
	public class ProductRepository : GenricRepository<Product>, IProductRepository
	{
		private readonly AppDbContext dbContext;
		private readonly IMapper mapper;
		private readonly IImageManagementService imageManagementService;

		public ProductRepository(AppDbContext dbContext,IMapper mapper,IImageManagementService imageManagementService)
			: base(dbContext)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			this.imageManagementService = imageManagementService;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
		{
			var query = dbContext.Products.Include(x => x.Photos).Include(x => x.Category).AsNoTracking();

			if (!string.IsNullOrEmpty(productParams.Search))
			{
				var seaarchWords = productParams.Search.Split(" ");
				query = query.Where(x => seaarchWords.All(
					word =>
					x.Name.ToLower().Contains(word.ToLower())
				    || 
				    x.Description.ToLower().Contains(word.ToLower())
				));
			}

			if (productParams.CategoryId.HasValue)
			{
				query = query.Where(x => x.CategoryId == productParams.CategoryId);
			}

			if (!string.IsNullOrEmpty(productParams.Sort))
			{
				query = productParams.Sort switch
				{
					"Price" => query.OrderBy(x => x.NewPrice),
					"PriceDesc" => query.OrderByDescending(x => x.NewPrice),
					_ => query.OrderBy(x => x.Id),
				};
			}

			query = query.Skip((productParams.PageNumber - 1) * productParams.pageSize).Take(productParams.pageSize);

		    var result = mapper.Map<List<ProductDTO>>(query);

			return result;
		}

		public async Task<bool> AddAsync(AddProductDTO addProductDTO)
		{
			if (addProductDTO == null) return false;

			var product = mapper.Map<Product>(addProductDTO);
			await dbContext.Products.AddAsync(product);
			await dbContext.SaveChangesAsync();

			var ImagesPath= await imageManagementService.AddImageAsync(addProductDTO.Photos, addProductDTO.Name);
			var photos = ImagesPath.Select(
				path => new Photo
				{
					ProductId = product.Id,
					ImageName = path 
				}).ToList();

			await dbContext.Photos.AddRangeAsync(photos);
			await dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
		{
			if (updateProductDTO == null) return false;

			var FoundProduct =await dbContext.Products.Include(x => x.Photos).Include(x => x.Category)
				              .FirstOrDefaultAsync(x => x.Id == updateProductDTO.Id);

			if (FoundProduct == null) return false;

			mapper.Map(updateProductDTO, FoundProduct);

			var FoundPhotos = await dbContext.Photos.Where(x => x.ProductId == updateProductDTO.Id).ToListAsync();
			foreach (var photo in FoundPhotos)
			{
				 imageManagementService.DeleteImageAsync(photo.ImageName);
			}

			dbContext.Photos.RemoveRange(FoundPhotos);

			var ImagesPath = await imageManagementService.AddImageAsync(updateProductDTO.Photos, updateProductDTO.Name);
			var photos = ImagesPath.Select(
				path => new Photo
				{
					ProductId = updateProductDTO.Id,
					ImageName = path
				}).ToList();

			await dbContext.Photos.AddRangeAsync(photos);
			await dbContext.SaveChangesAsync();
			return true;

		}
		public Task DeleteAsync(Product product)
		{
			var Photos =  dbContext.Photos.Where(x => x.ProductId == product.Id).ToList();
			foreach (var photo in Photos)
			{
				imageManagementService.DeleteImageAsync(photo.ImageName);
			}

			dbContext.Products.Remove(product);
			return dbContext.SaveChangesAsync();
		}
	}
}
