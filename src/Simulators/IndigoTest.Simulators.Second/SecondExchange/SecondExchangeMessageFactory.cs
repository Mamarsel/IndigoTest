using System.Globalization;
using System.Text.Json;
using IndigoTest.Simulators.Common.Contracts;
using IndigoTest.Simulators.Common.MarketData;

namespace IndigoTest.Simulators.Second.SecondExchange;

internal sealed class SecondExchangeMessageFactory : ISimulatorMessageFactory
{
    public string Create(SimulatedTick tick)
    {
        var message = new SecondExchangeMessage
        {
            Symbol = tick.Symbol,
            Price = tick.Price.ToString("0.00", CultureInfo.InvariantCulture),
            Volume = tick.Volume.ToString("0.0000", CultureInfo.InvariantCulture),
            Timestamp = tick.Timestamp.ToUnixTimeMilliseconds(),
            Kind = "trade",
        };

        return JsonSerializer.Serialize(message);
    }
}
