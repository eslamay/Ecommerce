using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure
{
	public static class infrastructureRegisteration
	{
		public static IServiceCollection InfastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddScoped(typeof(IGenricRepository<>), typeof(GenricRepository<>));
			// Applying the Unit of Work Pattern
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			//Add DbContext
			services.AddDbContext<AppDbContext>(op =>
			{
               op.UseSqlServer(configuration.GetConnectionString("constr"));
			});
			return services;
		}
	}
}
