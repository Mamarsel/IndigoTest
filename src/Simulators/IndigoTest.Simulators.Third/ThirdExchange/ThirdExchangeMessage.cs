using System.Text.Json.Serialization;

namespace IndigoTest.Simulators.Third.ThirdExchange;

internal sealed class ThirdExchangeMessage
{
    [JsonPropertyName("instrument")]
    public required string Instrument { get; init; }

    [JsonPropertyName("trade")]
    public required ThirdExchangeTrade Trade { get; init; }

    [JsonPropertyName("meta")]
    public required ThirdExchangeMeta Meta { get; init; }
}

internal sealed class ThirdExchangeTrade
{
    [JsonPropertyName("last")]
    public required decimal Last { get; init; }

    [JsonPropertyName("qty")]
    public required decimal Quantity { get; init; }
}

internal sealed class ThirdExchangeMeta
{
    [JsonPropertyName("time")]
    public required string Time { get; init; }

    [JsonPropertyName("source")]
    public required string Source { get; init; }
}
