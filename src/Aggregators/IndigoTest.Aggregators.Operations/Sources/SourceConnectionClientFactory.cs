namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed class SourceConnectionClientFactory : ISourceConnectionClientFactory
{
    public ISourceConnectionClient Create(SourceDefinition source)
    {
        return new WebSocketSourceConnectionClient(source);
    }
}
