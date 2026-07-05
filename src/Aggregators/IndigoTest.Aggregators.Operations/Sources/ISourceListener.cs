namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceListener
{
    Task RunAsync(SourceDefinition source, CancellationToken ct);
}
