namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed class SourceMessageParserResolver : ISourceMessageParserResolver
{
    private readonly IReadOnlyDictionary<string, ISourceMessageParser> parsers;

    public SourceMessageParserResolver(IEnumerable<ISourceMessageParser> parsers)
    {
        this.parsers = parsers.ToDictionary(
            parser => parser.Kind,
            StringComparer.OrdinalIgnoreCase);
    }

    public ISourceMessageParser Resolve(SourceDefinition source)
    {
        return parsers.TryGetValue(source.Kind, out var parser)
            ? parser
            : throw new InvalidOperationException($"Unknown source kind '{source.Kind}'.");
    }
}
