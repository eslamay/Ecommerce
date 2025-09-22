using Ecommerce.Core.DTO;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService orderService;

		public OrdersController(IOrderService orderService)
		{
			this.orderService = orderService;
		}

		[HttpPost("create-order")]
		public async Task<IActionResult> CreateOrder(OrderDtO orderDTO)
		{
			var email = User.FindFirst(ClaimTypes.Email)?.Value;

			var order=await orderService.CreateOrderAsync(orderDTO, email);

			return Ok(order);
		}

		[HttpGet("get-orders-for-user")]
		public async Task<IActionResult> GetOrdersForUser()
		{
			var email = User.FindFirst(ClaimTypes.Email)?.Value;
			var orders = await orderService.GetAllOrdersForUserAsync(email);
			return Ok(orders);
		}

		[HttpGet("get-order-by-id/{id}")]
		public async Task<IActionResult> GetOrderById(int id)
		{
			var email = User.FindFirst(ClaimTypes.Email)?.Value;
			var order = await orderService.GetOrderByIdAsync(id, email);
			return Ok(order);
		}

		[HttpGet("get-delivery")]
		public async Task<IActionResult> GetDelivery()
		{
			var delivery = await orderService.GetDeliveryMethodsAsync();
			return Ok(delivery);
		}
	}
}
