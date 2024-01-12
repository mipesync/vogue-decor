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
        
        builder.HasData(
            new { Id = 1, Name = "Золотой", EngName = "Gold" },
            new { Id = 2, Name = "Бронзовый", EngName = "Bronze" },
            new { Id = 3, Name = "Серебристый", EngName = "Silver" },
            new { Id = 4, Name = "Никель", EngName = "Nickel" },
            new { Id = 5, Name = "Хром", EngName = "Chrome" },
            new { Id = 6, Name = "Белый", EngName = "White" },
            new { Id = 7, Name = "Чёрный", EngName = "Black" },
            new { Id = 8, Name = "Прозрачный", EngName = "Clear" },
            new { Id = 9, Name = "Бежевый", EngName = "Beige" },
            new { Id = 10, Name = "Голубой", EngName = "Light blue" },
            new { Id = 11, Name = "Жёлтый", EngName = "Yellow" },
            new { Id = 12, Name = "Зелёный", EngName = "Green" },
            new { Id = 13, Name = "Коричневый", EngName = "Brown" },
            new { Id = 14, Name = "Красный", EngName = "Red" },
            new { Id = 15, Name = "Оранжевый", EngName = "Orange" },
            new { Id = 16, Name = "Розовый", EngName = "Pink" },
            new { Id = 17, Name = "Серый", EngName = "Gray" },
            new { Id = 18, Name = "Синий", EngName = "Blue" },
            new { Id = 19, Name = "Фиолетовый", EngName = "Purple" }
        );
    }
}