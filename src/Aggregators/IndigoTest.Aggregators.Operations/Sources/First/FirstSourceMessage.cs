using System.Text.Json.Serialization;

namespace IndigoTest.Aggregators.Operations.Sources.First;

public sealed class FirstSourceMessage
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
