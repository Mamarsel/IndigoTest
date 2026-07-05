namespace IndigoTest.Aggregators.Persistence;

public static class Constants
{
    public static class V1
    {
        public static class MarketTick
        {
            public const string Table = "Aggregator_MarketTick";

            public const string ID = "ID";

            public const string Symbol = "Symbol";

            public const string Price = "Price";

            public const string Volume = "Volume";

            public const string Timestamp = "Timestamp";

            public const string Source = "Source";

            public const string ReceivedAt = "ReceivedAt";
        }
    }
}
