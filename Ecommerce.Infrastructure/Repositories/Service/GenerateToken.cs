using Ecommerce.Core.Entities;
using Ecommerce.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructure.Repositories.Service
{
	public class GenerateToken : IGenerateToken
	{
		private readonly IConfiguration _configuration;

		public GenerateToken(IConfiguration configuration )
		{
			_configuration = configuration;
		}
		public string GetAndCreateToken(AppUser user)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
			};

			var securityKey = _configuration["Token:Secret"];
			var key = Encoding.ASCII.GetBytes(securityKey);
			SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
			
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				Issuer = _configuration["Token:Issuer"],
				SigningCredentials = signingCredentials,
				NotBefore = DateTime.Now
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
