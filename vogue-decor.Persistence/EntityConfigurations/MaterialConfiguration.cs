using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vogue_decor.Domain;

namespace vogue_decor.Persistence.EntityConfigurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");

        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.Id).IsUnique();
        
        builder.HasGeneratedTsVectorColumn(
                s => s.SearchVector,
                "russian",
                p => new { p.Name })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");

        builder.HasMany(s => s.Products)
            .WithMany(p => p.MaterialsObj)
            .UsingEntity<ProductMaterial>(
                j => j
                    .HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductMaterials)
                    .HasForeignKey(ps => ps.ProductId),
                o => o
                    .HasOne(ps => ps.Material)
                    .WithMany(s => s.ProductMaterials)
                    .HasForeignKey(ps => ps.MaterialId),
                p =>
                {
                    p.HasKey(new[] { "ProductId", "MaterialId" });
                    p.ToTable("ProductMaterials");
                });
        
        builder.HasData(
            new { Id = 1, Name = "Металлический" },
            new { Id = 2, Name = "Стеклянный" },
            new { Id = 3, Name = "Деревянный" },
            new { Id = 4, Name = "Смешанный" },
            new { Id = 5, Name = "Шёлковый" },
            new { Id = 6, Name = "Арт-шёлк" },
            new { Id = 7, Name = "Шерстяной" },
            new { Id = 8, Name = "Вискозный" },
            new { Id = 9, Name = "Искусственная нить" },
            new { Id = 10, Name = "Хлоповый" },
            new { Id = 11, Name = "Искусственный" },
            new { Id = 13, Name = "Бумажный" },
            new { Id = 14, Name = "Пластиковый" },
            new { Id = 15, Name = "Гипсовый" },
            new { Id = 16, Name = "Полиэстер" },
            new { Id = 17, Name = "Латунь" },
            new { Id = 18, Name = "Бронза" }
        );
    }
}