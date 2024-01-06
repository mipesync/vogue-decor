using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations
{
    /// <summary>
    /// Класс конфигурации таблицы пользователя
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.Name).HasMaxLength(32);

            builder.Property(x => x.Role).IsRequired();

            builder.Property(x => x.Email).IsRequired();

            builder.HasMany(u => u.Products)
                .WithMany(u => u.Users)
                .UsingEntity<Favourite>(
                j => j
                    .HasOne(u => u.Product)
                    .WithMany(u => u.Favourites)
                    .HasForeignKey(u => u.ProductId),
                o => o
                    .HasOne(u => u.User)
                    .WithMany(u => u.Favourites)
                    .HasForeignKey(u => u.UserId),
                p =>
                {
                    p.HasKey(new string[] { "ProductId", "UserId" });
                    p.ToTable("Favourites");
                });

            builder.HasMany(u => u.Logs).WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);
        }
    }
}
