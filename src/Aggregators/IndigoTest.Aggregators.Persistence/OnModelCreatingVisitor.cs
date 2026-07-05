using IndigoTest.Aggregators.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IndigoTest.Aggregators.Persistence;

internal sealed class OnModelCreatingVisitor : IOnModelCreatingVisitor
{
    public void Apply(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new MarketTickTypeConfiguration());
    }
}
