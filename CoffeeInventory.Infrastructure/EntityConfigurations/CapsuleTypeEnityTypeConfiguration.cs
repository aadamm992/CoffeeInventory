using CoffeeInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeInventory.Infrastructure.EntityConfigurations;

internal sealed class CapsuleTypeEnityTypeConfiguration : IEntityTypeConfiguration<CapsuleType>
{
    public void Configure(EntityTypeBuilder<CapsuleType> builder)
    {
        builder.Property(capsuleType => capsuleType.Name)
            .IsRequired()
            .HasMaxLength(20);
    }
}
