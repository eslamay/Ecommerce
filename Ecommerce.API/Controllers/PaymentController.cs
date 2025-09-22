using Ecommerce.Core.Entities;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			this.paymentService = paymentService;
		}

		[HttpPost("create")]
		public async Task<ActionResult<CustomerBasket>> Create(string basketId,int?deliveryId)
		{
			return await paymentService.CreateOrUpdatePaymentAsync(basketId, deliveryId);
		}
	}
}
