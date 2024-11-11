using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.DBContexts;
using TiendaOnline.Domain.Stocks;
using TiendaOnline.Infrastructure.DBContexts;
using TiendaOnline.Infrastructure.Repositories.PostgreSQL;
using TiendaOnline.Infrastructure.Repositories.Redis;
using TiendaOnline.Infrastructure.Repositories.Replicators;

namespace TiendaOnline;

public static class DependencyInjection
{
    /// <summary>
    /// Service for Application Adds.
    /// </summary>
    /// <param name="services">Services</param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(
       this IServiceCollection services
    )
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(
            configuration 
                => configuration
                .RegisterServicesFromAssembly(assembly)
        );

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }

    /// <summary>
    /// Service for Infrastructure Adds.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <param name="configuration">configurations.</param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("TiendaOnlineConnectionString")
             ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppEnvironmentDbContext>(options => {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddDbContext<RedisDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<AppEnvironmentDbContext>()
        );
                
        services.AddTransient<ServiceReplicator>();
        services.AddHostedService<ServiceCDC>();
        services.AddScoped<IStock, StockRepository>();

        return services; 
    }
}
