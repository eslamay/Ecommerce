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
	public class ProductsController : BaseController
	{
		public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
		{
		}

		[HttpGet("get-all")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var products = await work.ProductRepository.GetAllAsync(x=>x.Category,x=>x.Photos);
				var Result = mapper.Map<List<ProductDTO>>(products);
				if (products == null) return NotFound(new ResponseAPI(404, "No products found"));
				return Ok(Result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("get-by-id/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var product = await work.ProductRepository.GetByIdAsync(id,x=>x.Category,x=>x.Photos);
				var Result = mapper.Map<ProductDTO>(product);
				if (product == null) return NotFound(new ResponseAPI(404, "No product found"));
				return Ok(Result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("add-product")]
		public async Task<IActionResult> AddProduct(AddProductDTO addProductDTO)
		{
			try
			{
			  await work.ProductRepository.AddAsync(addProductDTO);
				return Ok(new ResponseAPI(200, "Product added successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseAPI(400, ex.Message));
			}
		}

		[HttpPut("update-product")]
		public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO)
		{
			try
			{
				await work.ProductRepository.UpdateAsync(updateProductDTO);
				return Ok(new ResponseAPI(200, "Product updated successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseAPI(400, ex.Message));
			}
		}

		[HttpDelete("delete-product/{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			try
			{
				var product = await work.ProductRepository.GetByIdAsync(id,x=>x.Photos,x=>x.Category);
				if (product == null) return NotFound(new ResponseAPI(404, "No product found"));
				await work.ProductRepository.DeleteAsync(product);
				return Ok(new ResponseAPI(200, "Product deleted successfully"));
			}
			catch (Exception ex)
			{
				return BadRequest(new ResponseAPI(400, ex.Message));
			}
		}
	}
}
