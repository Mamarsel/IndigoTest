using System.Text.Json.Serialization;

namespace IndigoTest.Simulators.First.FirstExchange;

internal sealed class FirstExchangeMessage
{
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    [JsonPropertyName("price")]
    public required decimal Price { get; init; }

    [JsonPropertyName("volume")]
    public required decimal Volume { get; init; }

    [JsonPropertyName("timestamp")]
    public required DateTimeOffset Timestamp { get; init; }
}
