using AutoMapper;
using Ecommerce.API.Helper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : BaseController
	{
		public CategoriesController(IUnitOfWork work,IMapper mapper) : base(work, mapper)
		{
		}

		[HttpGet("get-all")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var categories = await work.CategoryRepository.GetAllAsync();
				if (categories == null) return NotFound(new ResponseAPI(404, "No categories found"));
				return Ok(categories);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("get-by-id/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var category = await work.CategoryRepository.GetByIdAsync(id);
				if (category == null) return NotFound(new ResponseAPI(404, "No category found"));
				return Ok(category);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("add-category")]
		public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
		{
			try
			{
               var category = mapper.Map<Category>(categoryDTO);

				await work.CategoryRepository.AddAsync(category);
				return Ok(new ResponseAPI(200, "Category added successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("update-category")]
		public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO updateCategory)
		{
			try
			{
                var category = mapper.Map<Category>(updateCategory);
				await work.CategoryRepository.UpdateAsync(category);
				return Ok(new ResponseAPI(200, "Category updated successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("delete-category/{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			try
			{
				await work.CategoryRepository.DeleteAsync(id);
				return Ok(new ResponseAPI(200, "Category deleted successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
