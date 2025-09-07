﻿using Ecommerce.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Config
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
            builder.Property(x => x.Name).IsRequired();
			builder.Property(x => x.Description).IsRequired();
            builder.Property(x=>x.Price).HasColumnType("decimal(18,2)");
			builder.HasData(new Product { Id = 1, Name = "Laptop", Description = "Laptop", Price = 1000, CategoryId = 1 });
		}
	}
}
