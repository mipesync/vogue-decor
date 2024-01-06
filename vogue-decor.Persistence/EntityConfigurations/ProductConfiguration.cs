using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations
{
    /// <summary>
    /// Класс конфигурации таблицы товара
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder
                .HasIndex(p => p.SearchVector)
                .HasMethod("GIN");
            builder.Property(p => p.SearchVector).ValueGeneratedOnAddOrUpdate();
        }
    }
}
