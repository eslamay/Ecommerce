using Ecommerce.API.Middleware;
using Ecommerce.Infrastructure;
using Microsoft.OpenApi.Models;
namespace Ecommerce.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddMemoryCache();
			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen(
			   c =>
			   {
				   c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
				   {
					   Type = SecuritySchemeType.Http,
					   Scheme = "bearer",
				   });

				   c.AddSecurityRequirement(new OpenApiSecurityRequirement
				   {
					   {
						 new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
							},
							[]
					   }
				   });
			   }
			   );
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.InfastructureConfiguration(builder.Configuration);
			builder.Services.AddAutoMapper(cfg =>
			{
				cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
			});

			// CORS configuration
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("CORSPolicy",
				builder =>
				{
					builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			// Enable CORS
			app.UseCors("CORSPolicy");

			app.UseMiddleware<ExceptionsMiddleware>();
			
			app.UseAuthentication();

			app.UseAuthorization();

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();
			
			app.UseStaticFiles();

			app.MapControllers();

			app.Run();
		}
	}
}
