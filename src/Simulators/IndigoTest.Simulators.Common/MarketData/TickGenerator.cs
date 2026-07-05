using IndigoTest.Simulators.Common.Configuration;
using Microsoft.Extensions.Options;

namespace IndigoTest.Simulators.Common.MarketData;

public sealed class TickGenerator(IOptions<SimulatorOptions> options)
{
    private readonly object sync = new();

    private readonly SimulatorOptions options = options.Value;

    private readonly Dictionary<string, InstrumentState> instruments = options.Value.Symbols
        .Distinct(StringComparer.Ordinal)
        .ToDictionary(
            symbol => symbol,
            symbol => new InstrumentState
            {
                Symbol = symbol,
                Price = ResolveInitialPrice(symbol),
            },
            StringComparer.Ordinal);

    public SimulatedTick Generate()
    {
        var symbol = options.Symbols[Random.Shared.Next(options.Symbols.Length)];
        var instrument = instruments[symbol];
        var priceDelta = NextDecimal(-options.PriceStep, options.PriceStep);

        instrument.Price = Math.Max(0.01m, instrument.Price + priceDelta);

        var volume = NextDecimal(options.MinVolume, options.MaxVolume);

        return new SimulatedTick(instrument.Symbol,
            decimal.Round(instrument.Price, 2, MidpointRounding.AwayFromZero),
            decimal.Round(volume, 4, MidpointRounding.AwayFromZero),
            DateTimeOffset.UtcNow);
    }

    private static decimal ResolveInitialPrice(string symbol)
    {
        return symbol switch
        {
            "BTCUSDT" => 67000m,
            "ETHUSDT" => 3400m,
            "SOLUSDT" => 150m,
            "XRPUSDT" => 0.55m,
            "DOGEUSDT" => 0.12m,
            _ => 100m,
        };
    }

    private static decimal NextDecimal(decimal minValue, decimal maxValue)
    {
        var range = maxValue - minValue;
        var sample = (decimal)Random.Shared.NextDouble();

        return minValue + (range * sample);
    }
}
