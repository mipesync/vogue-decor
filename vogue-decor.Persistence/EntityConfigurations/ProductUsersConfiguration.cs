using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace vogue_decor.Persistence.EntityConfigurations
{
    public class ProductUsersConfiguration : IEntityTypeConfiguration<ProductUser>
    {
        public void Configure(EntityTypeBuilder<ProductUser> builder)
        {
            builder.ToTable("ProductUsers");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.ProductId).IsRequired();

            builder.HasOne(x => x.Product).WithMany(x => x.ProductUsers);
            builder.HasOne(x => x.User).WithMany(x => x.ProductUsers);
        }
    }
}
