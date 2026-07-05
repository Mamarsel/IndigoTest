using IndigoTest.Aggregator.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndigoTest.Aggregators.Persistence;

public interface IAggregatorDataScope
{
    DbSet<MarketTick> MarketTicks { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
