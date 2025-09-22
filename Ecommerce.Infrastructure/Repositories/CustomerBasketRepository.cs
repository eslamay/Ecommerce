using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ecommerce.Infrastructure.Repositories
{
	public class CustomerBasketRepository : ICustomerBasketRepository
	{
		private readonly IDatabase database;
		public CustomerBasketRepository(IConnectionMultiplexer redis) 
		{
             database = redis.GetDatabase();
		}
		public async Task<CustomerBasket> GetBasketAsync(string id)
		{
			var result=await database.StringGetAsync(id);
			if(!string.IsNullOrEmpty(result))
			{
				return JsonSerializer.Deserialize<CustomerBasket>(result);
			}
			return null;
		}

		public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
		{
			var _basket= await database.StringSetAsync(basket.Id
				,JsonSerializer.Serialize(basket),TimeSpan.FromDays(4));

			if(_basket)
			{
				return await GetBasketAsync(basket.Id);
			}
			return null;
		}
		public Task<bool> DeleteBasketAsync(string basketId)
		{
			return database.KeyDeleteAsync(basketId);
		}

	}
}
