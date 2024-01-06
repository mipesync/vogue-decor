using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace vogue_decor.Services
{
    public static class SwaggerService
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LightningShop - API для интернет-магазина по продаже люстр",
                    Description = "Документация по использованию LightningShopAPI.\n" +
                    "Описание всех объектов находится в самом низу страницы"
                });

                var subProjectAssemblies = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(u => u.FullName!.Contains("vogue-decor")).ToArray();

                foreach (var subProject in subProjectAssemblies )
                {
                    var xmlFile = $"{subProject.GetName().Name}.xml";
                    var assemblyRootPath = Directory.GetParent(subProject.Location)!.FullName;
                    var xmlPath = Path.Combine(assemblyRootPath, xmlFile);
                    config.IncludeXmlComments(xmlPath);
                }

                config.IncludeXmlCommentsFromInheritDocs(includeRemarks: true, excludedTypes: typeof(string));

                config.AddEnumsWithValuesFixFilters();

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Поместите токен доступа в поле ниже",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }
}
