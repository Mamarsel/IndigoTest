namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceConnectionClientFactory
{
    ISourceConnectionClient Create(SourceDefinition source);
}
