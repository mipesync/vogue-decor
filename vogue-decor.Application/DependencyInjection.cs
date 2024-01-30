using vogue_decor.Application.Common.Managers;
using vogue_decor.Application.Common.Services;
using vogue_decor.Application.DTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace vogue_decor.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services,
            JwtOptionsDto jwtOptions)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<ICollectionRepository, CollectionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFiltersRepository, FiltersRepository>();
            services.AddTransient<IFilesRepository, FilesRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IProductCodeGenerator, ProductCodeGenerator>();

            var dbContext = services.BuildServiceProvider().GetService<IDBContext>();

            if (dbContext is not null)
                services.AddTransient<ITokenManager>(x => new TokenManager(jwtOptions));
        }
    }
}
