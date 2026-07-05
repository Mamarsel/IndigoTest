using IndigoTest.Aggregator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IndigoTest.Aggregators.Persistence.Configurations;

internal sealed class MarketTickTypeConfiguration : IEntityTypeConfiguration<MarketTick>
{
    public void Configure(EntityTypeBuilder<MarketTick> builder)
    {
        builder.ToTable(Constants.V1.MarketTick.Table);

        builder.HasKey(x => x.ID);

        builder
            .Property(x => x.Price)
            .HasPrecision(18, 8);

        builder
            .Property(x => x.Volume)
            .HasPrecision(18, 8);

        builder.HasIndex(x => new
        {
            x.Source,
            x.Timestamp,
        });

        builder.HasIndex(x => new
        {
            x.Symbol,
            x.Timestamp,
        });
    }
}
