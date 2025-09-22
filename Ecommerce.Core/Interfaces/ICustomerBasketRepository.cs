using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
	public interface ICustomerBasketRepository
	{
		Task<CustomerBasket> GetBasketAsync(string id);
		Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
		Task<bool> DeleteBasketAsync(string basketId);
	}
}
