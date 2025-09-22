using Ecommerce.Core.Entities.Order;

namespace Ecommerce.Core.DTO
{
	public class OrderToReturnDTO
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; }
		public decimal SubTotal { get; set; }
		public decimal Total { get; set; }
		public DateTime OrderDate { get; set; }
		public ShippingAddress shippingAddress { get; set; }

		public IReadOnlyList<OrderItemDTO> orderItems { get; set; }
		public string deliveryMethod { get; set; }


		public string status { get; set; }
	}

	public class OrderItemDTO
	{
		public int ProductItemId { get; set; }
		public string MainImage { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
