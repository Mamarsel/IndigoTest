namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceMessageParserResolver
{
    ISourceMessageParser Resolve(SourceDefinition source);
}
