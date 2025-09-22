using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Core.Sharing;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Ecommerce.Infrastructure.Repositories
{
	public class AuthRepository:IAuth
	{
		private readonly UserManager<AppUser> userManager;
		private readonly SignInManager<AppUser> signInManager;
		private readonly ISendEmail sendEmail;
		private readonly IGenerateToken token;
		private readonly AppDbContext dbContext;
		public AuthRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ISendEmail sendEmail, IGenerateToken token, AppDbContext dbContext)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.sendEmail = sendEmail;
			this.token = token;
			this.dbContext = dbContext;
		}

		public async Task<string> RegisterAsync(RegisterDTO registerDTO)
		{
			if (registerDTO == null) return null;

			if(await userManager.FindByEmailAsync(registerDTO.Email) != null)
			{
				return "Email is already in use";
			}

			if (await userManager.FindByNameAsync(registerDTO.Name) != null)
			{
				return "Username is already in use";
			}

			AppUser appUser = new AppUser()
			{
				Email = registerDTO.Email,
				UserName = registerDTO.Name,
				DisplayName = registerDTO.DisplayName
			};
			
			var result = await userManager.CreateAsync(appUser, registerDTO.Password);
			if (!result.Succeeded)
			{
				return result.Errors.ToList()[0].Description;
			}
			//send Activation Email
			 var token = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
             await SendEmail(appUser.Email, "Activation Email", "Activation", "Activate your account", token);
			
			return "Success";
		}

		public async Task SendEmail(string email, string subject, string component, string message, string code)
		{
			var result = new EmailDTO(
				email,
				 "YourEmail",
				subject,
				EmailStringBody.Send(email, code, component, message)
			);

			await sendEmail.SendEmailAsync(result);
		}

		public async Task<string> LoginAsync(LoginDTO loginDTO)
		{
			if (loginDTO == null) return null;

			var findUser = await userManager.FindByEmailAsync(loginDTO.Email);

			if (!findUser.EmailConfirmed)
			{
				var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
				await SendEmail(findUser.Email, "Activation Email", "Activation", "Activate your account", token);

				return "please check your email to activate your account";
			}

			var result= await signInManager.CheckPasswordSignInAsync(findUser, loginDTO.Password, true);

			if (result.Succeeded)
			{
				return token.GetAndCreateToken(findUser);
			}
			return"please check your email and password";
		}

		public async Task<bool> SendEmailForForgotPasswordAsync(string email)
		{
			var findUser = await userManager.FindByEmailAsync(email);
			if (findUser is null)
			{
				return false;
			}
			var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
			await SendEmail(findUser.Email, "Reset Email", "Reset-Password", "Reset your password", token);
			return true;
		}

		public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
		{
			var findUser = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
			if (findUser is null)
			{
				return null;
			}

			var result = await userManager.ResetPasswordAsync(findUser, resetPasswordDTO.Token, resetPasswordDTO.Password);
			
			if (result.Succeeded) {
				return "done";
			}
			return result.Errors.ToList()[0].Description;
		}

		public async Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDTO)
		{
			var findUser = await userManager.FindByEmailAsync(activeAccountDTO.Email);
			if (findUser is null)
			{
				return false;
			}
			var result = await userManager.ConfirmEmailAsync(findUser, activeAccountDTO.Token);
			if (result.Succeeded)
			{
				return true;
			}

			var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
			await SendEmail(findUser.Email, "Activation Email", "Activation", "Activate your account", token);
 
            return false;
		}

		public async Task<bool> UpdateAddress(string email, Address address)
		{
			var findUser = await userManager.FindByEmailAsync(email);
			if (findUser is null)
			{
				return false;
			}

			var myAddress= await dbContext.Addresses.FirstOrDefaultAsync(x => x.AppUserId == findUser.Id);
			if (myAddress is null)
			{
				address.AppUserId = findUser.Id;
				await dbContext.Addresses.AddAsync(address);
			}
			else
			{
				address.Id = myAddress.Id;
				dbContext.Addresses.Update(address);
			}
			await dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<Address> GetUserAddress(string email)
		{
			var User =await userManager.FindByEmailAsync(email);
			if (User is null)
			{
				return null;
			}
			var address = await dbContext.Addresses.FirstOrDefaultAsync(x => x.AppUserId == User.Id);
			return address;
		}
	}
}
