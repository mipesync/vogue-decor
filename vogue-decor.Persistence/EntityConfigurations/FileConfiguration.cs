using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = vogue_decor.Domain.File;

namespace vogue_decor.Persistence.EntityConfigurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("Files");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Id).IsUnique();

        builder.HasIndex(u => u.Name).IsUnique();
    }
}