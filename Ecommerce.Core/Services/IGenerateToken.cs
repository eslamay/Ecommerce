using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services
{
	public interface IGenerateToken
	{
		string GetAndCreateToken(AppUser user);
	}
}
