using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация таблицы цветов
/// </summary>
public class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("Colors");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name).IsRequired();

        builder.HasGeneratedTsVectorColumn(
                p => p.SearchVector,
                "russian",
                p => new { p.Name, p.EngName })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");
    }
}