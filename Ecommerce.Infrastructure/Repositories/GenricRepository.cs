using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Repositories
{
	public class GenricRepository<T> : IGenricRepository<T> where T : class
	{
		private readonly AppDbContext dbContext;

		public GenricRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await dbContext.Set<T>().AsNoTracking().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
		{
			var query = dbContext.Set<T>().AsQueryable();

			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			return await query.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			var entity = await dbContext.Set<T>().FindAsync(id);
			return entity;
		}

		public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
		{
			var query = dbContext.Set<T>().AsQueryable();

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			var entity =await query.FirstOrDefaultAsync(x=>EF.Property<int>(x, "Id") == id);
			return entity;
		}

		public async Task AddAsync(T entity)
		{
			await dbContext.Set<T>().AddAsync(entity);
			await dbContext.SaveChangesAsync();
		}
		public async Task UpdateAsync(T entity)
		{
			dbContext.Entry(entity).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
		public async Task DeleteAsync(int id)
		{
			var entity = await dbContext.Set<T>().FindAsync(id);
			dbContext.Set<T>().Remove(entity);
			await dbContext.SaveChangesAsync();
		}	
	}
}
