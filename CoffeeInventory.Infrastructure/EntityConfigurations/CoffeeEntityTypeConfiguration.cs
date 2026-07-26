using CoffeeInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeInventory.Infrastructure.EntityConfigurations;

internal sealed class CoffeeEntityTypeConfiguration : IEntityTypeConfiguration<Coffee>
{
    public void Configure(EntityTypeBuilder<Coffee> builder)
    {

        builder.Property(coffee => coffee.Name)
               .IsRequired()
               .HasMaxLength(50);

        // Coffee -> Brand (many-to-one)
        builder.HasOne(coffee => coffee.Brand)
            .WithMany(brand => brand.Coffees)
            .HasForeignKey(coffee => coffee.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        // Coffee -> CapsuleType (many-to-one)
        builder.HasOne(coffee => coffee.CapsuleType)
            .WithMany(capsuleType => capsuleType.Coffees)
            .HasForeignKey(coffee => coffee.CapsuleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-many relationship configuration between Coffee and CupSize
        builder.HasMany(coffee => coffee.CupSizes)
            .WithMany(cupSize => cupSize.Coffees)
            .UsingEntity(
                "CoffeeCupSizes",
                right => right.HasOne(typeof(CupSize))
                    .WithMany()
                    .HasForeignKey("CupSizeId")
                    .OnDelete(DeleteBehavior.Restrict),
                left => left.HasOne(typeof(Coffee))
                    .WithMany()
                    .HasForeignKey("CoffeeId")
                    .OnDelete(DeleteBehavior.Restrict)
            );
    }
}
