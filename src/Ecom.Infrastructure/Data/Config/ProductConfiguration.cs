using Ecom.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Id).HasMaxLength(30);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            
            //seed
            builder.HasData(
                new Product { Id = 1, Name = "Product_1", Description = "P1", Price = 3000, CategoryId = 1, ProductPicture = "https://" },
                new Product { Id = 2, Name = "Product_2", Description = "P2", Price = 2000, CategoryId = 2, ProductPicture = "https://" },
                new Product { Id = 3, Name = "Product_3", Description = "P3", Price = 1000, CategoryId = 3, ProductPicture = "https://" }
                );
        }
    }
}
