﻿using vogue_decor.Application.Interfaces;
using vogue_decor.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using File = vogue_decor.Domain.File;

namespace vogue_decor.Persistence
{
    /// <inheritdoc/>
    public class DBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IDBContext
    {
        /// <summary>
        /// Конструктор, инициализирующий первоначальные настройки контекста
        /// </summary>
        /// <param name="options">Первоначальные настройки</param>
        public DBContext(DbContextOptions<DBContext> options): base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductUser> ProductUsers { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ChandelierType> ChandelierTypes { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
