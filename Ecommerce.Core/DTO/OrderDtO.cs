using Ecommerce.Core.Entities.Order;

namespace Ecommerce.Core.DTO
{
	public class OrderDtO
	{
		public int deliveryMethodId { get; set; }
		public string basketId { get; set; }
		public ShippingAddressDTO shippingAddress { get; set; }
	}

	public class ShippingAddressDTO
	{
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string street { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zipCode { get; set; }
	}
}
