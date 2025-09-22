using AutoMapper;
using Ecommerce.API.Helper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : BaseController
	{
		public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
		{
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			var result = await work.auth.RegisterAsync(registerDTO);
			if (result!= "Success")
			{ 
				return BadRequest(new ResponseAPI(400, result));
			}
			return Ok(new ResponseAPI(200, "User registered successfully"));
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			var result = await work.auth.LoginAsync(loginDTO);
			if (result.StartsWith("please"))
			{
				return BadRequest(new ResponseAPI(400, result));
			}

			Response.Cookies.Append("token", result,
				new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					Domain = "localhost",
					Expires = DateTime.Now.AddDays(1),
					SameSite = SameSiteMode.None,
					IsEssential = true
				});
			return Ok(new ResponseAPI(200, "User logged in successfully"));
		}

		[HttpPost("active-account")]
		public async Task<IActionResult> ActiveAccount(ActiveAccountDTO activeAccountDTO)
		{
			var result = await work.auth.ActiveAccount(activeAccountDTO);
			
			return result ? Ok(new ResponseAPI(200, "Account activated successfully"))
				: BadRequest(new ResponseAPI(400, "Problem in activating account"));
		}

		[HttpGet("send-email-forgot-password")]
		public async Task<IActionResult> SendEmailForgotPassword(string email)
		{
			var result = await work.auth.SendEmailForForgotPasswordAsync(email);
			return result ? Ok(new ResponseAPI(200, "Email sent successfully"))
				: BadRequest(new ResponseAPI(400, "Problem in sending email"));
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> reset(ResetPasswordDTO restPasswordDTO)
		{
			var result = await work.auth.ResetPassword(restPasswordDTO);
			if (result == "done")
			{
				return Ok(new ResponseAPI(200));
			}
			return BadRequest(new ResponseAPI(400, result));
		}
		[Authorize]
		[HttpPut("update-address")]
		public async Task<IActionResult> UpdateAddress(ShippingAddressDTO addressDTO)
		{
			var email = User.FindFirst(ClaimTypes.Email)?.Value;
			var address = mapper.Map<Address>(addressDTO);
			var result = await work.auth.UpdateAddress(email,address);
			return result ? Ok(new ResponseAPI(200, "Address updated successfully"))
				: BadRequest(new ResponseAPI(400, "Problem in updating address"));
		}

		[HttpGet("IsUserAuth")]
		public async Task<IActionResult> IsUserAuth()
		{
			return User.Identity.IsAuthenticated?
				Ok(new ResponseAPI(200, "User is authenticated")) 
				: BadRequest(new ResponseAPI(400, "User is not authenticated"));
		}

		[Authorize]
		[HttpGet("get-address-for-user")]
		public async Task<IActionResult> GetAddressForUser()
		{
			var address = await work.auth.GetUserAddress(User.FindFirst(ClaimTypes.Email)?.Value);
			var result = mapper.Map<ShippingAddressDTO>(address);
			return result == null ? BadRequest(new ResponseAPI(400, "Problem in getting address")) : Ok(result);
		}
	}
}
