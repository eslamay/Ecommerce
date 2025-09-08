using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Ecommerce.Infrastructure
{
	public static class infrastructureRegisteration
	{
		public static IServiceCollection InfastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddScoped(typeof(IGenricRepository<>), typeof(GenricRepository<>));
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
			return services;
		}
	}
}
