using AutoMapper;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BugController : BaseController
	{
		public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
		{
		}
		[HttpGet("not-found")]
		public async Task<IActionResult> GetNotFound()
		{
			var category =await work.CategoryRepository.GetByIdAsync(42);
			if (category == null) return NotFound(new { message = "Category not found" });
			return Ok(category);
		}
		[HttpGet("server-error")]
		public async Task<IActionResult> GetServerError()
		{
			var category =await work.CategoryRepository.GetByIdAsync(42);
			category.Name = "";
			return Ok(category);
		}
		[HttpGet("bad-request/{Id}")]
		public async Task<IActionResult> GetBadRequest(int id)
		{
			return Ok();
		}
		[HttpGet("bad-request/")]
		public async Task<IActionResult> GetBadRequest()
		{
			return BadRequest();
		}
	}
}
