using vogue_decor.Application.Common.Options;
using vogue_decor.Persistence;

namespace vogue_decor.Services
{
    public static class PersistenceService
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services,
            string connectionString, IConfiguration configuration, ILogger logger)
        {
            services.AddPersistence(connectionString,
                new EmailSenderOptions
                {
                    Name = configuration["SMTP:Name"]!,
                    Domain = configuration["SMTP:Domain"]!,
                    Username = configuration["SMTP:Username"]!,
                    Password = configuration["SMTP:Password"]!,
                    Host = configuration["SMTP:Host"]!,
                    Port = Convert.ToInt32(configuration["SMTP:Port"]),
                    UseSSL = Convert.ToBoolean(configuration["SMTP:UseSSL"])
                }, logger);

            return services;
        }
    }
}
