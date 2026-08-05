using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Ignore(p => p.DomainEvents);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Sku).IsRequired().HasMaxLength(50);

        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(m => m.Amount).HasColumnName("Price").HasPrecision(18, 2);
            price.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
        });

        builder.HasIndex(p => p.Sku).IsUnique();
    }
}
