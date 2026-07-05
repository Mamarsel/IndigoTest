using System.Text.Json;
using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Sources.First;

internal sealed class FirstSourceMessageParser : ISourceMessageParser
{
    public string Kind => "first";

    public NormalizedTick? Parse(ReceivedSourceMessage message, SourceDefinition source)
    {
        var sourceMessage = JsonSerializer.Deserialize<FirstSourceMessage>(message.Payload);

        if (sourceMessage == null)
        {
            return null;
        }

        return new NormalizedTick()
        {
            Price = sourceMessage.Price,
            Source = source.Source,
            ReceivedAt = message.ReceivedAt,
            Symbol = sourceMessage.Symbol,
            Timestamp = sourceMessage.Timestamp,
            Volume = sourceMessage.Volume,
        };
    }
}
