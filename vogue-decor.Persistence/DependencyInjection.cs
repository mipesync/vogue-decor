using vogue_decor.Application.Common.Options;
using vogue_decor.Application.Interfaces;
using vogue_decor.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace vogue_decor.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, 
            string connectionString, EmailSenderOptions emailSenderOptions)
        {
            services.AddDbContext<DBContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IDBContext>(provider => provider.GetService<DBContext>()!);
            services.AddScoped<IEmailSender>(x => new EmailSender(emailSenderOptions));
            services.AddScoped<IFileUploader, FileUploader>();

            return services;
        }
    }
}
