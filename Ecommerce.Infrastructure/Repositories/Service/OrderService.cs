using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories.Service
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly AppDbContext dbContext;
		private readonly IMapper mapper;
		private readonly IPaymentService paymentService;

		public OrderService(AppDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
		{
			this.dbContext = dbContext;
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.paymentService = paymentService;
		}
		public async Task<Orders> CreateOrderAsync(OrderDtO orderDtO, string BuyerEmail)
		{
			var basket = await unitOfWork.customerBasketRepository.GetBasketAsync(orderDtO.basketId);

			if (basket == null || basket.basketItems == null || !basket.basketItems.Any())
			{
				throw new Exception("Basket is empty or not found.");
			}

			List<OrderItem> orderItems = new List<OrderItem>();
			foreach (var item in basket.basketItems)
			{
				var product=await unitOfWork.ProductRepository.GetByIdAsync(item.Id);
				var orderItem=new OrderItem(product.Id,product.Name,item.Image,item.Price,item.Quantity);
				orderItems.Add(orderItem);
			}

			var deliveryMethod=await dbContext.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == orderDtO.deliveryMethodId);

			var subtotal=orderItems.Sum(x => x.Price * x.Quantity);

			var ship=mapper.Map<ShippingAddress>(orderDtO.shippingAddress);

			var ExistOrder=await dbContext.Orders.Where(x=>x.PaymentIntentId==basket.PaymentIntentId).FirstOrDefaultAsync();

            if (ExistOrder!=null)
			{
				dbContext.Orders.Remove(ExistOrder);
				await paymentService.CreateOrUpdatePaymentAsync(basket.PaymentIntentId,deliveryMethod.Id);
			}

			var order=new Orders(BuyerEmail,subtotal,ship,deliveryMethod!,orderItems,basket.PaymentIntentId);
			await dbContext.Orders.AddAsync(order);
			await dbContext.SaveChangesAsync();

			await unitOfWork.customerBasketRepository.DeleteBasketAsync(orderDtO.basketId);
			return order;
		}

		public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string buyerEmail)
		{
			var orders =await dbContext.Orders.Where(x => x.BuyerEmail == buyerEmail)
				        .Include(x => x.orderItems).Include(x => x.deliveryMethod).ToListAsync();

			var result = mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);

			return result;
		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			return await dbContext.DeliveryMethods.AsNoTracking().ToListAsync();
		}

		public async Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail)
		{
			var order=await dbContext.Orders.Where(x=>x.Id==id&&x.BuyerEmail==buyerEmail)
                     .Include(x => x.orderItems).Include(x=>x.deliveryMethod).FirstOrDefaultAsync();

			var result = mapper.Map<OrderToReturnDTO>(order);

			return result;
		}
	}
}
