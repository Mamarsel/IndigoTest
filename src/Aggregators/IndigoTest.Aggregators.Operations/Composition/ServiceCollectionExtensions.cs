using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Processing;
using IndigoTest.Aggregators.Operations.Sources;
using IndigoTest.Aggregators.Operations.Sources.First;
using IndigoTest.Aggregators.Operations.Sources.Second;
using IndigoTest.Aggregators.Operations.Sources.Third;
using IndigoTest.Aggregators.Operations.Status;
using IndigoTest.Aggregators.Operations.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IndigoTest.Aggregators.Operations.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAggregatorOperations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<AggregatorOptions>()
            .BindConfiguration(AggregatorOptions.Section);

        services.AddSingleton<IAggregatorStatus, AggregatorStatusTracker>();

        services.AddTransient<ISourceMessageParser, FirstSourceMessageParser>();
        services.AddTransient<ISourceMessageParser, SecondSourceMessageParser>();
        services.AddTransient<ISourceMessageParser, ThirdSourceMessageParser>();

        services.AddTransient<ISourceMessageParserResolver, SourceMessageParserResolver>();
        services.AddTransient<ISourceConnectionClientFactory, SourceConnectionClientFactory>();
        services.AddTransient<ISourceReconnectPolicy, SourceReconnectPolicy>();
        services.AddTransient<ISourceLivenessMonitorFactory, SourceLivenessMonitorFactory>();
        services.AddTransient<ISourceListener, SourceListener>();
        services.AddSingleton<ITickTransport, TickTransport>();
        services.AddSingleton<ITickDeduplicator, TickDeduplicator>();
        services.AddTransient<ITickProcessor, TickProcessor>();
        services.AddHostedService<TickPipelineHostedService>();
        services.AddHostedService<SourceListenerHostedService>();

        return services;
    }
}
