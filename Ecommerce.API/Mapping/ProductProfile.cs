using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;

namespace Ecommerce.API.Mapping
{
	public class ProductProfile:Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductDTO>()
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();

			CreateMap<AddProductDTO, Product>().ForMember(m=>m.Photos,opt=>opt.Ignore()).ReverseMap();

			CreateMap<UpdateProductDTO, Product>().ForMember(m => m.Photos, opt => opt.Ignore()).ReverseMap();

			CreateMap<Photo, PhotoDTO>().ReverseMap();
		}
	}
}
