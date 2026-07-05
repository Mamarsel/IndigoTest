using IndigoTest.Aggregator.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndigoTest.Aggregators.Persistence;

internal sealed class AggregatorDataScope(DbContext context) : IAggregatorDataScope
{
    private readonly DbContext context = context;

    public DbSet<MarketTick> MarketTicks => context.Set<MarketTick>();

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return context.SaveChangesAsync(ct);
    }
}
