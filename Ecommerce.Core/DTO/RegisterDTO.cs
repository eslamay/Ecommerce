namespace Ecommerce.Core.DTO
{
	public class LoginDTO
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
	public class RegisterDTO:LoginDTO
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
	}
    public class ResetPasswordDTO:LoginDTO
	{
        public string Token { get; set; }
	}
	public class ActiveAccountDTO
	{ 
		public string Email { get; set; }
		public string Token { get; set; }
	}

}
