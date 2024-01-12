using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vogue_decor.Domain;

namespace vogue_decor.Persistence.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id).IsUnique();
        
        builder.HasGeneratedTsVectorColumn(
                s => s.SearchVector,
                "russian",
                p => new { p.Name })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");
        
        builder.HasData(
            new { Id = 1, Name = "Люстры", ProductTypeId = 1 },
            new { Id = 2, Name = "Бра", ProductTypeId = 1 },
            new { Id = 3, Name = "Настольные лампы", ProductTypeId = 1 },
            new { Id = 4, Name = "Торшеры", ProductTypeId = 1 },
            new { Id = 5, Name = "Подвесные светильники", ProductTypeId = 1 },
            new { Id = 6, Name = "Потолочные светильники", ProductTypeId = 1 },
            new { Id = 7, Name = "Уличный свет", ProductTypeId = 1 },
            new { Id = 8, Name = "Подсветка для картин", ProductTypeId = 1 },
            new { Id = 9, Name = "Треки и споты", ProductTypeId = 1 },
            new { Id = 10, Name = "Аксессуары к светильникам", ProductTypeId = 1 },
            new { Id = 11, Name = "Диваны", ProductTypeId = 2 },
            new { Id = 12, Name = "Кресла", ProductTypeId = 2 },
            new { Id = 13, Name = "Столы", ProductTypeId = 2 },
            new { Id = 14, Name = "Стулья", ProductTypeId = 2 },
            new { Id = 15, Name = "Комоды", ProductTypeId = 2 },
            new { Id = 16, Name = "Консоли", ProductTypeId = 2 },
            new { Id = 17, Name = "Кровати", ProductTypeId = 2 },
            new { Id = 18, Name = "Матрасы", ProductTypeId = 2 },
            new { Id = 19, Name = "Пуфы и банкетки", ProductTypeId = 2 },
            new { Id = 20, Name = "Панно / арт объект", ProductTypeId = 3 },
            new { Id = 21, Name = "С принтами", ProductTypeId = 3 },
            new { Id = 22, Name = "Солнышко", ProductTypeId = 3 },
            new { Id = 23, Name = "С деревом", ProductTypeId = 3 },
            new { Id = 24, Name = "Дизайнерские с металлом", ProductTypeId = 3 },
            new { Id = 25, Name = "Классические", ProductTypeId = 3 },
            new { Id = 26, Name = "Настольные", ProductTypeId = 3 },
            new { Id = 27, Name = "Напольные", ProductTypeId = 3 },
            new { Id = 28, Name = "Прямоугольные", ProductTypeId = 3 },
            new { Id = 29, Name = "Круглые", ProductTypeId = 3 },
            new { Id = 30, Name = "Прямоугольные", ProductTypeId = 4 },
            new { Id = 31, Name = "Квадратные", ProductTypeId = 4 },
            new { Id = 32, Name = "Круглые", ProductTypeId = 4 },
            new { Id = 33, Name = "Овальные", ProductTypeId = 4 },
            new { Id = 34, Name = "Дорожки", ProductTypeId = 4 },
            new { Id = 35, Name = "Нестандартные", ProductTypeId = 4 },
            new { Id = 36, Name = "Дизайнерские тарелки", ProductTypeId = 5 },
            new { Id = 37, Name = "Стремянки и скамьи", ProductTypeId = 5 },
            new { Id = 38, Name = "Сушилки", ProductTypeId = 5 },
            new { Id = 39, Name = "Гладильные доски", ProductTypeId = 5 },
            new { Id = 40, Name = "Вешалки напольные", ProductTypeId = 5 },
            new { Id = 41, Name = "Вешалки настенные", ProductTypeId = 5 },
            new { Id = 42, Name = "Аксессуары для ванной", ProductTypeId = 5 },
            new { Id = 43, Name = "Ложки для обуви", ProductTypeId = 5 },
            new { Id = 44, Name = "Вазы и подсвечники", ProductTypeId = 5 },
            new { Id = 45, Name = "Декоративные подушки", ProductTypeId = 5 },
            new { Id = 46, Name = "Пледы", ProductTypeId = 5 },
            new { Id = 47, Name = "Покрывала", ProductTypeId = 5 },
            new { Id = 48, Name = "Современные игрушки и статуэтки", ProductTypeId = 6 },
            new { Id = 49, Name = "Часы", ProductTypeId = 6 },
            new { Id = 50, Name = "Настольные и настенные игры", ProductTypeId = 6 },
            new { Id = 51, Name = "Зонты", ProductTypeId = 6 },
            new { Id = 52, Name = "Подставки для зонтов", ProductTypeId = 6 },
            new { Id = 53, Name = "Ложки для обуви", ProductTypeId = 6 },
            new { Id = 54, Name = "Держатели книг", ProductTypeId = 6 },
            new { Id = 55, Name = "Арт-объекты", ProductTypeId = 7 },
            new { Id = 56, Name = "Картины авторские", ProductTypeId = 7 },
            new { Id = 57, Name = "Постеры", ProductTypeId = 7 },
            new { Id = 58, Name = "Панно с лего", ProductTypeId = 7 },
            new { Id = 59, Name = "Панно в спортивном стиле", ProductTypeId = 7 },
            new { Id = 60, Name = "Репродукция", ProductTypeId = 7 },
            new { Id = 61, Name = "Абажуры", ProductTypeId = 8 },
            new { Id = 62, Name = "Колпаки и крепления", ProductTypeId = 8 },
            new { Id = 63, Name = "Лампочки", ProductTypeId = 8 },
            new { Id = 64, Name = "Светодиодные ленты и подсветки", ProductTypeId = 8 }
        );
    }
}