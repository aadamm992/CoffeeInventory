using CoffeeInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeInventory.Infrastructure.EntityConfigurations;

internal sealed class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(brand => brand.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
