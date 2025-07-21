using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tajan.ProductServcie.Domain.Entities;

namespace Tajan.ProductService.Infrastructure.Persistence.Configurations;

internal sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Name)
             .IsRequired()
             .HasMaxLength(100);
    }
}