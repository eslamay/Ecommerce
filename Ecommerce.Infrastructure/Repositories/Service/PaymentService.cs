using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Ecommerce.Infrastructure.Repositories.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly AppDbContext dbContext;
		public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, AppDbContext dbContext)
		{
			this.unitOfWork = unitOfWork;
			_configuration = configuration;
			this.dbContext = dbContext;
		}

		public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId,int? deliveryMethodId)
		{
			var basket=await unitOfWork.customerBasketRepository.GetBasketAsync(basketId);
			StripeConfiguration.ApiKey = _configuration["StripeSettings:secretKey"];
            var shippingPrice = 0m;

			if (deliveryMethodId.HasValue)
			{
				var deliveryMethod = await dbContext.DeliveryMethods.FindAsync(deliveryMethodId);
				if (deliveryMethod != null)
				{
					shippingPrice = deliveryMethod.Price;
				}
			}

			foreach (var item in basket.basketItems)
			{
				var product =await unitOfWork.ProductRepository.GetByIdAsync(item.Id);
				item.Price = product.NewPrice;
			}
			PaymentIntentService paymentIntentService = new PaymentIntentService();
			PaymentIntent intent;

			if (string.IsNullOrEmpty(basket.PaymentIntentId)|| string.IsNullOrEmpty(basket.ClientSecret))
			{
				var options = new PaymentIntentCreateOptions
				{
					Amount=(long)basket.basketItems.Sum(x => x.Quantity * (x.Price*100)) + (long)(shippingPrice*100),
					Currency = "usd",
					PaymentMethodTypes = new List<string> { "card" }
				};
				intent = await paymentIntentService.CreateAsync(options);
				basket.PaymentIntentId = intent.Id;
				basket.ClientSecret = intent.ClientSecret;
			}
			else
			{
				var options = new PaymentIntentUpdateOptions
				{
					Amount = (long)basket.basketItems.Sum(x => x.Quantity * (x.Price * 100)) + (long)(shippingPrice * 100)
				};
				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
			}

			await unitOfWork.customerBasketRepository.UpdateBasketAsync(basket);
			return basket;

		}
		public async Task<Orders> UpdateOrderSuccess(string PaymentInten)
		{
			var order = await dbContext.Orders.FirstOrDefaultAsync(m => m.PaymentIntentId == PaymentInten);
			if (order is null)
			{
				return null;
			}
			order.status = Status.PaymentReceived;
			dbContext.Orders.Update(order);
			await dbContext.SaveChangesAsync();
			return order;
		}
		public async Task<Orders> UpdateOrderFaild(string PaymentInten)
		{
			var order = await dbContext.Orders.FirstOrDefaultAsync(m => m.PaymentIntentId == PaymentInten);
			if (order is null)
			{
				return null;
			}
			order.status = Status.PaymentFailed;
			dbContext.Orders.Update(order);
			await dbContext.SaveChangesAsync();
			return order;
		}
	}
}
