using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vogue_decor.Domain;

namespace vogue_decor.Persistence.EntityConfigurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Id).IsUnique();

        builder.HasMany(b => b.Products)
            .WithOne(p => p.Brand)
            .HasForeignKey(p => p.BrandId);

        builder.HasMany(b => b.Collections)
            .WithOne(c => c.Brand)
            .HasForeignKey(c => c.BrandId);
    }
}