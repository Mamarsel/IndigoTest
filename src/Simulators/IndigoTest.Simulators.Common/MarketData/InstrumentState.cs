namespace IndigoTest.Simulators.Common.MarketData;

internal sealed class InstrumentState
{
    public required string Symbol { get; init; }

    public decimal Price { get; set; }
}
