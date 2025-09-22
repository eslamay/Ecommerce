using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Core.Entities
{
	public class AppUser:IdentityUser
	{
		public string DisplayName { get; set; }
		public Address Address { get; set; }
	}
}
