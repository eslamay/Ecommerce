namespace Ecommerce.API.Helper
{
	public class ApiException : ResponseAPI
	{
		public ApiException(int statusCode,string details ,string message = null) : base(statusCode, message)
		{
			Details = details;
		}
		public string Details { get; set; }
	}
}
