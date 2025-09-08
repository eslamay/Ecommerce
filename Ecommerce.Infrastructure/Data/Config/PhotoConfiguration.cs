using Ecommerce.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Config
{
	public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
	{
		public void Configure(EntityTypeBuilder<Photo> builder)
		{
			builder.HasData(new Photo { Id = 1, ImageName = "laptop.jpg", ProductId = 1 });
		}
	}
}
