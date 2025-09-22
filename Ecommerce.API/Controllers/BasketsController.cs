using AutoMapper;
using Ecommerce.API.Helper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketsController : BaseController
	{
		public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
		{
		}

		[HttpGet("get-basket-item/{id}")]
		public async Task<IActionResult> GetBasketItem(string id)
		{
			try
			{
				var basketItem = await work.customerBasketRepository.GetBasketAsync(id);
				if (basketItem == null) return Ok(new CustomerBasket());
				return Ok(basketItem);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("update-basket")]
		public async Task<IActionResult>AddBasketItem(CustomerBasket customerBasket)
		{
			if (string.IsNullOrWhiteSpace(customerBasket.Id) || customerBasket.Id == "string")
			{
				customerBasket.Id = Guid.NewGuid().ToString();
			}
			var basket = await work.customerBasketRepository.UpdateBasketAsync(customerBasket);
			if(basket==null) return BadRequest(new ResponseAPI(400, "Problem in updating basket"));
			return Ok(basket);
		}

		[HttpDelete("delete-basket/{id}")]
		public async Task<IActionResult>DeleteBasketItem(string id)
		{
			var result= await work.customerBasketRepository.DeleteBasketAsync(id);
			if(result) return Ok(new ResponseAPI(200,"Basket deleted successfully"));
			return BadRequest(new ResponseAPI(400,"Problem in deleting basket"));
		}
	}
}
