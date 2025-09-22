using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
	public interface IAuth
	{
		Task<string> RegisterAsync(RegisterDTO registerDTO);
		Task SendEmail(string email, string subject, string component, string message, string code);
	    Task<string> LoginAsync(LoginDTO loginDTO);
		Task<bool> SendEmailForForgotPasswordAsync(string email);
		Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
		Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDTO);
		Task<bool>UpdateAddress(string email,Address address);
		Task<Address> GetUserAddress(string email);

	}
}
