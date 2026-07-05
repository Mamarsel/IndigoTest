using System.Text.Json.Serialization;

namespace IndigoTest.Aggregators.Operations.Sources.Third;

public sealed class ThirdSourceMessage
{
    [JsonPropertyName("instrument")]
    public required string Instrument { get; init; }

    [JsonPropertyName("trade")]
    public required ThirdSourceTradeMessage Trade { get; init; }

    [JsonPropertyName("meta")]
    public required ThirdSourceMetaMessage Meta { get; init; }
}

public sealed class ThirdSourceTradeMessage
{
    [JsonPropertyName("last")]
    public required decimal Last { get; init; }

    [JsonPropertyName("qty")]
    public required decimal Quantity { get; init; }
}

public sealed class ThirdSourceMetaMessage
{
    [JsonPropertyName("time")]
    public required string Time { get; init; }

    [JsonPropertyName("source")]
    public required string Source { get; init; }
}
