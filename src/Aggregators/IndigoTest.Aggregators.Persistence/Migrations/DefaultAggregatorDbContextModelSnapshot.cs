using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace IndigoTest.Aggregators.Persistence.Migrations;

[DbContext(typeof(DefaultAggregatorDbContext))]
partial class DefaultAggregatorDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("IndigoTest.Aggregator.Domain.MarketTick", b =>
            {
                b.Property<Guid>("ID")
                    .HasColumnType("uuid")
                    .HasColumnName("ID");

                b.Property<decimal>("Price")
                    .HasPrecision(18, 8)
                    .HasColumnType("numeric(18,8)")
                    .HasColumnName("Price");

                b.Property<DateTimeOffset>("ReceivedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("ReceivedAt");

                b.Property<string>("Source")
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("Source");

                b.Property<string>("Symbol")
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("Symbol");

                b.Property<DateTimeOffset>("Timestamp")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("Timestamp");

                b.Property<decimal>("Volume")
                    .HasPrecision(18, 8)
                    .HasColumnType("numeric(18,8)")
                    .HasColumnName("Volume");

                b.HasKey("ID");

                b.HasIndex("Source", "Timestamp");

                b.HasIndex("Symbol", "Timestamp");

                b.ToTable("Aggregator_MarketTick");
            });
#pragma warning restore 612, 618
    }
}
