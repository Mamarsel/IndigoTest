using Microsoft.EntityFrameworkCore;

namespace IndigoTest.Aggregators.Persistence;

internal sealed class DefaultAggregatorDbContext(
    DbContextOptions options,
    IEnumerable<IOnModelCreatingVisitor> visitors)
    : DbContext(options)
{
    private readonly IEnumerable<IOnModelCreatingVisitor> visitors = visitors;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var visitor in visitors)
        {
            visitor.Apply(builder);
        }
    }
}
