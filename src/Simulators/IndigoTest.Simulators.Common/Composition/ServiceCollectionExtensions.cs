using System.Text.Json.Serialization;
using IndigoTest.Simulators.Common.Configuration;
using IndigoTest.Simulators.Common.Contracts;
using IndigoTest.Simulators.Common.Faults;
using IndigoTest.Simulators.Common.MarketData;
using IndigoTest.Simulators.Common.Status;
using IndigoTest.Simulators.Common.Streaming;
using Microsoft.Extensions.DependencyInjection;

namespace IndigoTest.Simulators.Common.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSimulator<TMessageFactory>(
        this IServiceCollection services)
        where TMessageFactory : class, ISimulatorMessageFactory
    {
        services
            .AddOptions<SimulatorOptions>()
            .BindConfiguration(SimulatorOptions.Section)
            .Validate(ValidateOptions, "Simulator options are invalid.")
            .ValidateOnStart();

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddSingleton<TickGenerator>();
        services.AddSingleton<SimulatorStateStore>();
        services.AddSingleton<SimulatorStatusProvider>();
        services.AddSingleton<WebSocketConnectionHandler>();
        services.AddSingleton<ISimulatorMessageFactory, TMessageFactory>();

        return services;
    }

    private static bool ValidateOptions(SimulatorOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.SourceName))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(options.WebSocketPath))
        {
            return false;
        }

        if (options.TickIntervalMs <= 0)
        {
            return false;
        }

        if (options.TicksPerIteration <= 0)
        {
            return false;
        }

        if (options.Symbols is not { Length: > 0 })
        {
            return false;
        }

        if (options.MinVolume <= 0 || options.MaxVolume < options.MinVolume)
        {
            return false;
        }

        return options.PriceStep > 0;
    }
}
