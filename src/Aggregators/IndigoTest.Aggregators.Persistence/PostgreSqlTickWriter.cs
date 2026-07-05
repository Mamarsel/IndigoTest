using IndigoTest.Aggregators.Operations.Ticks;
using IndigoTest.Aggregators.Operations.Writers;
using Npgsql;
using NpgsqlTypes;

namespace IndigoTest.Aggregators.Persistence;

internal sealed class PostgreSqlTickWriter(NpgsqlDataSource dataSource)
    : ITickWriter
{
    private readonly NpgsqlDataSource dataSource = dataSource;

    public async Task WriteAsync(TickWriteBatch batch, CancellationToken ct)
    {
        if (batch.Items.Count == 0)
        {
            return;
        }

        await using var connection = await dataSource.OpenConnectionAsync(ct);

        await using var importer = await connection.BeginBinaryImportAsync(
            $"""
            COPY "{Constants.V1.MarketTick.Table}"
                ("{Constants.V1.MarketTick.ID}", "{Constants.V1.MarketTick.Symbol}", "{Constants.V1.MarketTick.Price}",
                 "{Constants.V1.MarketTick.Volume}", "{Constants.V1.MarketTick.Timestamp}", "{Constants.V1.MarketTick.Source}",
                 "{Constants.V1.MarketTick.ReceivedAt}")
            FROM STDIN (FORMAT BINARY)
            """,
            ct);

        foreach (var tick in batch.Items)
        {
            await importer.StartRowAsync(ct);

            await importer.WriteAsync(Guid.NewGuid(), NpgsqlDbType.Uuid, ct);
            await importer.WriteAsync(tick.Symbol, NpgsqlDbType.Text, ct);
            await importer.WriteAsync(tick.Price, NpgsqlDbType.Numeric, ct);
            await importer.WriteAsync(tick.Volume, NpgsqlDbType.Numeric, ct);
            await importer.WriteAsync(tick.Timestamp, NpgsqlDbType.TimestampTz, ct);
            await importer.WriteAsync(tick.Source, NpgsqlDbType.Text, ct);
            await importer.WriteAsync(tick.ReceivedAt, NpgsqlDbType.TimestampTz, ct);
        }

        await importer.CompleteAsync(ct);
    }
}
