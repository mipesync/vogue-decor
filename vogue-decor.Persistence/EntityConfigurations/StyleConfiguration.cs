using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vogue_decor.Domain;

namespace vogue_decor.Persistence.EntityConfigurations;

public class StyleConfiguration : IEntityTypeConfiguration<Style>
{
    public void Configure(EntityTypeBuilder<Style> builder)
    {
        builder.ToTable("Styles");

        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.Id).IsUnique();
        
        builder.HasGeneratedTsVectorColumn(
                s => s.SearchVector,
                "russian",
                p => new { p.Name })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");

        builder.HasMany(s => s.Products)
            .WithMany(p => p.StylesObj)
            .UsingEntity<ProductStyle>(
                j => j
                    .HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductStyles)
                    .HasForeignKey(ps => ps.ProductId),
                o => o
                    .HasOne(ps => ps.Style)
                    .WithMany(s => s.ProductStyles)
                    .HasForeignKey(ps => ps.StyleId),
                p =>
                {
                    p.HasKey(new[] { "ProductId", "StyleId" });
                    p.ToTable("ProductStyles");
                });
        
        builder.HasData(
            new { Id = 1, Name = "Классика" },
            new { Id = 2, Name = "Нео-классика" },
            new { Id = 3, Name = "Лофт" },
            new { Id = 4, Name = "Поп-Арт" },
            new { Id = 5, Name = "Модерн" },
            new { Id = 6, Name = "Арт-деко" },
            new { Id = 7, Name = "Минималист" },
            new { Id = 8, Name = "ХайТек" },
            new { Id = 9, Name = "Скандинавский" }
        );
    }
}