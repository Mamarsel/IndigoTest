namespace IndigoTest.Simulators.Common.MarketData;

public sealed record SimulatedTick(string Symbol,
    decimal Price,
    decimal Volume,
    DateTimeOffset Timestamp);
