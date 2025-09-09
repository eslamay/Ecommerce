using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Sharing;

namespace Ecommerce.Core.Interfaces
{
	public interface IProductRepository:IGenricRepository<Product>
	{
		Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
		Task<bool> AddAsync(AddProductDTO addProductDTOp);
		Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);
		Task DeleteAsync(Product product);
	}
}
