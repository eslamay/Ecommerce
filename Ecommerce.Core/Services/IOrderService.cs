using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Order;

namespace Ecommerce.Core.Services
{
	public interface IOrderService
	{
		Task<Orders> CreateOrderAsync(OrderDtO orderDtO, string BuyerEmail);
		Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string buyerEmail);
		Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
	}
}
