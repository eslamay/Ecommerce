using Ecommerce.Core.DTO;

namespace Ecommerce.Core.Services
{
	public interface ISendEmail
	{
		Task SendEmailAsync(EmailDTO emailDTO);
	}
}
