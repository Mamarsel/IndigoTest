using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndigoTest.Aggregators.Persistence.Migrations;

[DbContext(typeof(DefaultAggregatorDbContext))]
[Migration("202607030001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Aggregator_MarketTick",
            columns: table => new
            {
                ID = table.Column<Guid>(type: "uuid", nullable: false),
                Symbol = table.Column<string>(type: "text", nullable: false),
                Price = table.Column<decimal>(type: "numeric(18,8)", precision: 18, scale: 8, nullable: false),
                Volume = table.Column<decimal>(type: "numeric(18,8)", precision: 18, scale: 8, nullable: false),
                Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Source = table.Column<string>(type: "text", nullable: false),
                ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Aggregator_MarketTick", x => x.ID);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Aggregator_MarketTick_Source_Timestamp",
            table: "Aggregator_MarketTick",
            columns: new[]
            {
                "Source",
                "Timestamp",
            });

        migrationBuilder.CreateIndex(
            name: "IX_Aggregator_MarketTick_Symbol_Timestamp",
            table: "Aggregator_MarketTick",
            columns: new[]
            {
                "Symbol",
                "Timestamp",
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Aggregator_MarketTick");
    }
}
