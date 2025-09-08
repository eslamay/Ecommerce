using Ecommerce.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecommerce.API.Middleware
{
	public class ExceptionsMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IHostEnvironment environment;
		private readonly IMemoryCache memoryCache;
		private readonly TimeSpan rateLimitWindow = TimeSpan.FromSeconds(30);
		public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
		{
			_next = next;
			this.environment = environment;
			this.memoryCache = memoryCache;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				ApplySecuirty(context);

				if (!IsRequestAllowed(context))
				{
					context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
					context.Response.ContentType = "application/json";

					var response = new ApiException((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");

					await context.Response.WriteAsJsonAsync(response);
				}
				await _next(context);
			}
			catch (Exception ex)
			{
				context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var response =environment.IsDevelopment() ?
					new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
					:
					new ApiException((int)HttpStatusCode.InternalServerError,ex.Message);
				var json = JsonSerializer.Serialize(response);
				await context.Response.WriteAsync(json);
			}
		}

		private bool IsRequestAllowed(HttpContext context)
		{
			var ip = context.Connection.RemoteIpAddress.ToString();
			var cacheKey = $"Rate:{ip}";
			var dateNow = DateTime.Now;

			var (timespan, count) = memoryCache.GetOrCreate(cacheKey, entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = rateLimitWindow;
				return (timespan:dateNow,count:0);
			});

			if (dateNow-timespan < rateLimitWindow)
			{
				if (count > 8)
				{
					return false;
				}
                
					memoryCache.Set(cacheKey, (timespan, count += 1), rateLimitWindow);
			}
			else
			{
							memoryCache.Set(cacheKey, (timespan, count), rateLimitWindow);
			}

			return true;
		}

		private void ApplySecuirty(HttpContext context) 
		{
		   context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
			context.Response.Headers.Add("X-Frame-Options", "DENY");
			context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
		}
	}
}
