using System.Text.Json.Serialization;

namespace IndigoTest.Aggregators.Operations.Sources.Second;

public sealed class SecondSourceMessage
{
    [JsonPropertyName("s")]
    public required string Symbol { get; init; }

    [JsonPropertyName("p")]
    public required string Price { get; init; }

    [JsonPropertyName("v")]
    public required string Volume { get; init; }

    [JsonPropertyName("ts")]
    public required long Timestamp { get; init; }

    [JsonPropertyName("kind")]
    public required string Kind { get; init; }
}
