using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Order;

namespace Ecommerce.Core.Services
{
	public interface IPaymentService
	{
		Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, int? deliveryId);
		Task<Orders> UpdateOrderSuccess(string PaymentInten);
		Task<Orders> UpdateOrderFaild(string PaymentInten);
	}
}
