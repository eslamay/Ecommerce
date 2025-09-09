using Ecommerce.API.Middleware;
using Ecommerce.Infrastructure;
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

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
