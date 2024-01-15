﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vogue_decor.Domain;

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

        builder.HasData(
            new { Id = 1, Name = "Большие" },
            new { Id = 2, Name = "С хрусталями" },
            new { Id = 3, Name = "С абажурами" },
            new { Id = 4, Name = "Подвесные" },
            new { Id = 5, Name = "С потолочным креплением" },
            new { Id = 6, Name = "Овальные" } );
    }
}