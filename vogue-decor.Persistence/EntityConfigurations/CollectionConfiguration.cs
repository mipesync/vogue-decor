using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations
{
    /// <summary>
    /// Класс конфигурации таблицы коллекций
    /// </summary>
    public class CollectionConfiguration: IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.ToTable("Collections");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.Name).IsRequired();

            builder.HasMany(u => u.Products)
                .WithOne(u => u.Collection)
                .HasForeignKey(u => u.CollectionId);
        }
    }
}
