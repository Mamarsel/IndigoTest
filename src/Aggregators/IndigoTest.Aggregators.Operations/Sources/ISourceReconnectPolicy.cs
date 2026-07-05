namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceReconnectPolicy
{
    TimeSpan GetDelay(int attempt);
}
