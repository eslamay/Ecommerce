using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;

namespace Ecommerce.API.Mapping
{
	public class CategoryProfile:Profile
	{
		public CategoryProfile()
		{
			CreateMap<CategoryDTO,Category>().ReverseMap();
			CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
		}
	}
}
