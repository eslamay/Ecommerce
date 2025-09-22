using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Repositories.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Ecommerce.Infrastructure
{
	public static class infrastructureRegisteration
	{
		public static IServiceCollection InfastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddScoped(typeof(IGenricRepository<>), typeof(GenricRepository<>));
			services.AddScoped<ISendEmail, SendEmail>();
			services.AddScoped<IGenerateToken, GenerateToken>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IPaymentService, PaymentService>();

			//Apply redis
			services.AddSingleton<IConnectionMultiplexer>(i =>
			{
				var config=ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
			});
			// Applying the Unit of Work Pattern
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddSingleton<IImageManagementService, ImageManagementService>();
			services.AddSingleton<IFileProvider>(
	         new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);

			//Add DbContext
			services.AddDbContext<AppDbContext>(op =>
			{
               op.UseSqlServer(configuration.GetConnectionString("constr"));
			});

			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			services.AddAuthentication(
				op =>
				{
					op.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
					op.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
					op.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme;
				}).AddCookie(o =>
				{
					o.Cookie.Name = "token";
					o.Events.OnRedirectToLogin = context =>
					{
						context.Response.StatusCode = StatusCodes.Status401Unauthorized; ;
						return Task.CompletedTask;
					};
				}).AddJwtBearer(o =>
				{
					o.RequireHttpsMetadata = false;
					o.SaveToken = true;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Secret"])),
						ValidateIssuer = true,
						ValidIssuer = configuration["Token:Issuer"],
						ValidateAudience = false,
						ClockSkew = TimeSpan.Zero
					};
					o.Events = new JwtBearerEvents()
					{
						OnMessageReceived = context =>
						{
							var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
							if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
							{
								context.Token = authHeader.Substring("Bearer ".Length).Trim();
							}
							else
							{
								// Fallback to cookie
								var token = context.Request.Cookies["token"];
								context.Token = token;
							}
							return Task.CompletedTask;
						}
					};
				});
			return services;
		}
	}
}
