using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.Services
{
	public interface IImageManagementService
	{
		Task<List<string>> AddImageAsync(IFormFileCollection file, string src);
		void DeleteImageAsync(string src);
	}
}
