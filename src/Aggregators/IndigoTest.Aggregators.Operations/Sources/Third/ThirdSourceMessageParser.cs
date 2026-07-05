using System.Text.Json;
using System.Globalization;
using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Sources.Third;

internal sealed class ThirdSourceMessageParser : ISourceMessageParser
{
    public string Kind => "third";

    public NormalizedTick? Parse(ReceivedSourceMessage message, SourceDefinition source)
    {
        var sourceMessage = JsonSerializer.Deserialize<ThirdSourceMessage>(message.Payload);

        if (sourceMessage == null)
        {
            return null;
        }

        if (!DateTimeOffset.TryParseExact(sourceMessage.Meta.Time,
                "dd.MM.yyyy HH:mm:ss.fff",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var timestamp))
        {
            return null;
        }

        return new NormalizedTick()
        {
            Price = sourceMessage.Trade.Last,
            Source = source.Source,
            ReceivedAt = message.ReceivedAt,
            Symbol = sourceMessage.Instrument,
            Timestamp = timestamp,
            Volume = sourceMessage.Trade.Quantity,
        };
    }
}
