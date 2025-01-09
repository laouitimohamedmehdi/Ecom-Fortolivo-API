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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Id).HasMaxLength(30);

            //seed
            builder.HasData(
                new Category { Id = 1, Name = "Category_1", Description = "Huile extra vierge" },
                new Category { Id = 2, Name = "Category_2", Description = "Huile vierge" },
                new Category { Id = 3, Name = "Category_3", Description = "Huile normal" }
                );
        }
    }
}
