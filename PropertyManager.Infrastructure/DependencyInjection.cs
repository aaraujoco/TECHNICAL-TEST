using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Interfaces.Services;
using PropertyManager.Infrastructure.Persistence.Common;
using PropertyManager.Infrastructure.Persistence.Dapper;
using PropertyManager.Infrastructure.Services;

namespace PropertyManager.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepository(configuration);
    }

    private static void AddRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();
        services.AddScoped(typeof(DapperDbContext));
        services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
        services.AddTransient<IOwnerObjectRepository, OwnerObjectRepository>();
        services.AddTransient<IPropertyObjectRepository, PropertyObjectRepository>();
        services.AddTransient<IPropertyTraceObjectRepository, PropertyTraceObjectRepository>();
        services.AddTransient<IPropertyTraceObjectService, PropertyTraceObjectService>();
        services.AddTransient<IPropertyImageObjectRepository, PropertyImageObjectRepository>();

        services.AddHttpClient("PropertyTrace", config =>
        {
            config.BaseAddress = new Uri(configuration["Services:PropertyTrace"]);
        });
    }
}

