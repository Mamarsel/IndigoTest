using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Processing;

public interface ITickDeduplicator
{
    bool IsDuplicate(NormalizedTick tick);
}
