using Google.Protobuf.WellKnownTypes;
using IGaming.Core.DatabaseAccessHelpers;
using IGaming.Core.UsersManagement.Security;
using IGaming.Infrastructure.Security.Hashing;
using IGaming.Infrastructure.Security.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace IGaming.Infrastructure.Security.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtProvider(this IServiceCollection services, Action<JwtSettings> options)
        {
            services.Configure(options);
 
            return  services.AddSingleton<IJwtProvider, JwtProvider>();
        }
        public static IServiceCollection AddDbConnectionProvider(this IServiceCollection services, Action<DbConfig> options)
        {

            services.Configure(options);
            services.AddSingleton<IDbConnectionProvider, MySqlConnectionProvider>();
            return services;
        }

        public static IServiceCollection AddHasher(this IServiceCollection services)
        {
            services.AddSingleton<IHasher, Hasher>();
            return services;
        }
    }
}
