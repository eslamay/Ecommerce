using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Order;

namespace Ecommerce.API.Mapping
{
	public class OrderProfile:Profile
	{
        public OrderProfile()
		{
			CreateMap<Orders, OrderToReturnDTO>()
				.ForMember(d => d.deliveryMethod,
				o => o.
				MapFrom(s => s.deliveryMethod.Name))
				.ReverseMap();

			CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
			CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();

			CreateMap<Address,ShippingAddressDTO>().ReverseMap(); 
		}
	}
}
