using System.Text.Json;
using IndigoTest.Simulators.Common.Contracts;
using IndigoTest.Simulators.Common.MarketData;

namespace IndigoTest.Simulators.First.FirstExchange;

internal sealed class FirstExchangeMessageFactory : ISimulatorMessageFactory
{
    public string Create(SimulatedTick tick)
    {
        var message = new FirstExchangeMessage
        {
            Symbol = tick.Symbol,
            Price = tick.Price,
            Volume = tick.Volume,
            Timestamp = tick.Timestamp,
        };

        return JsonSerializer.Serialize(message);
    }
}
