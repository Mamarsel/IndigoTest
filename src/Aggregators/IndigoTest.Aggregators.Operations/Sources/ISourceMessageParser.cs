using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceMessageParser
{
    string Kind { get; }

    NormalizedTick? Parse(ReceivedSourceMessage message, SourceDefinition source);
}
