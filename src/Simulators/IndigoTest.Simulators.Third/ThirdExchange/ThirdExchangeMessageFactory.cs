using System.Globalization;
using System.Text.Json;
using IndigoTest.Simulators.Common.Contracts;
using IndigoTest.Simulators.Common.MarketData;

namespace IndigoTest.Simulators.Third.ThirdExchange;

internal sealed class ThirdExchangeMessageFactory : ISimulatorMessageFactory
{
    public string Create(SimulatedTick tick)
    {
        var message = new ThirdExchangeMessage
        {
            Instrument = tick.Symbol,
            Trade = new ThirdExchangeTrade
            {
                Last = tick.Price,
                Quantity = tick.Volume,
            },
            Meta = new ThirdExchangeMeta
            {
                Time = tick.Timestamp.ToString("dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture),
                Source = "third-exchange",
            },
        };

        return JsonSerializer.Serialize(message);
    }
}
