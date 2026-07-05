using System.Text.Json;
using System.Globalization;
using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Sources.Second;

internal sealed class SecondSourceMessageParser : ISourceMessageParser
{
    public string Kind => "second";

    public NormalizedTick? Parse(ReceivedSourceMessage message, SourceDefinition source)
    {
        var sourceMessage = JsonSerializer.Deserialize<SecondSourceMessage>(message.Payload);

        if (sourceMessage == null)
        {
            return null;
        }

        if (!decimal.TryParse(sourceMessage.Price, NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
        {
            return null;
        }

        if (!decimal.TryParse(sourceMessage.Volume, NumberStyles.Number, CultureInfo.InvariantCulture, out var volume))
        {
            return null;
        }

        return new NormalizedTick()
        {
            Price = price,
            Source = source.Source,
            ReceivedAt = message.ReceivedAt,
            Symbol = sourceMessage.Symbol,
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(sourceMessage.Timestamp),
            Volume = volume,
        };
    }
}
