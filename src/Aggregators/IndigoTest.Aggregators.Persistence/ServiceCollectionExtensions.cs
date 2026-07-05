using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IndigoTest.Aggregators.Operations.Writers;

namespace IndigoTest.Aggregators.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAggregatorPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(PersistenceConfiguration.ConnectionStringName);

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services
            .AddSingleton<IOnModelCreatingVisitor, OnModelCreatingVisitor>()
            .AddDbContext<DefaultAggregatorDbContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<DbContext, DefaultAggregatorDbContext>()
            .AddScoped<IAggregatorDataScope, AggregatorDataScope>()
            .AddNpgsqlDataSource(connectionString)
            .AddTransient<ITickWriter, PostgreSqlTickWriter>();

        return services;
    }
}
