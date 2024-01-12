using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация таблицы типов товара
/// </summary>
public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.ToTable("ProductTypes");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name).IsRequired();

        builder.HasGeneratedTsVectorColumn(
            p => p.SearchVector,
            "russian",
            p => new { p.Name })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");

        builder.HasData(
            new { Id = 1, Name = "Свет" },
            new { Id = 2, Name = "Мебель" },
            new { Id = 3, Name = "Зеркала" },
            new { Id = 4, Name = "Ковры" },
            new { Id = 5, Name = "Товары для дома" },
            new { Id = 6, Name = "Аксессуары" },
            new { Id = 7, Name = "Картины и пано" },
            new { Id = 8, Name = "Аксессуары к светильникам" }
        );
    }
}