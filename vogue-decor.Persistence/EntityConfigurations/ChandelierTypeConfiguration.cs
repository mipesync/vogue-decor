using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация таблицы типов люстр
/// </summary>
public class ChandelierTypeConfiguration : IEntityTypeConfiguration<ChandelierType>
{
    public void Configure(EntityTypeBuilder<ChandelierType> builder)
    {
        builder.ToTable("ChandelierTypes");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name).IsRequired();

        builder.HasGeneratedTsVectorColumn(
                p => p.SearchVector,
                "russian",
                p => new { p.Name })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");
    }
}