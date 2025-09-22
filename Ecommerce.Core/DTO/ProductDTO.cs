using Ecommerce.Core.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.DTO
{
	public record ProductDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal NewPrice { get; set; }
		public decimal OldPrice { get; set; }

		public virtual List<PhotoDTO> Photos { get; set; }
		public string CategoryName { get; set; }
	}

	public record ReturnProductDTO
	{
		public List<ProductDTO> Products { get; set; }
		public int TotalCount { get; set; }
	}

	public record AddProductDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal NewPrice { get; set; }
		public decimal OldPrice { get; set; }

		public IFormFileCollection Photos { get; set; }
		public int CategoryId { get; set; }
	}

	public record UpdateProductDTO: AddProductDTO
	{
		public int Id { get; set; }
	}

	public record PhotoDTO
	{
		public string ImageName { get; set; }
		public int ProductId { get; set; }
	}
}
