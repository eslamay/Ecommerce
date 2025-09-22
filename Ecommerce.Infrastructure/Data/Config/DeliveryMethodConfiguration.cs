using Ecommerce.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Config
{
	public class DeliveryMethodConfiguration: IEntityTypeConfiguration<DeliveryMethod>
	{
		public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
		{
			builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
			builder.HasData(new DeliveryMethod { Id = 1, Name = "DHL", DeliveryTime = "3-4 days", Description = "Delivery by DHL company", Price = 10 },
                           new DeliveryMethod { Id = 2, Name = "UPS", DeliveryTime = "2-3 days", Description = "Delivery by UPS company", Price = 12 },
						   new DeliveryMethod { Id = 3, Name = "FedEx", DeliveryTime = "1-2 days", Description = "Delivery by FedEx company", Price = 15 },
						   new DeliveryMethod { Id = 4, Name = "Ecommerce", DeliveryTime = "1 week", Description = "Delivery by Ecommerce company", Price = 0 }
				);
		}
	}
}
