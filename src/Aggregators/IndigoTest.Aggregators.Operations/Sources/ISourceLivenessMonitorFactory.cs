namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceLivenessMonitorFactory
{
    ISourceLivenessMonitor Create(SourceDefinition source);
}
