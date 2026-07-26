using CoffeeInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeInventory.Infrastructure.EntityConfigurations;

internal sealed class CupSizeEnityTypeConfiguration : IEntityTypeConfiguration<CupSize>
{
    public void Configure(EntityTypeBuilder<CupSize> builder)
    {
        builder.Property(cupSize => cupSize.Name)
            .IsRequired()
            .HasMaxLength(20);
    }
}
