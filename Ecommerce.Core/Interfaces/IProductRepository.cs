using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;

namespace Ecommerce.Core.Interfaces
{
	public interface IProductRepository:IGenricRepository<Product>
	{
		Task<bool> AddAsync(AddProductDTO addProductDTOp);
		Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);
		Task DeleteAsync(Product product);
	}
}
