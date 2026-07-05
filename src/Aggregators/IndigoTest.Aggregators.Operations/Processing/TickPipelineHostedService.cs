using IndigoTest.Aggregators.Operations.Transport;
using Microsoft.Extensions.Hosting;

namespace IndigoTest.Aggregators.Operations.Processing;

internal sealed class TickPipelineHostedService(
    ITickTransport transport,
    ITickProcessor processor)
    : IHostedService, IDisposable
{
    private readonly ITickTransport transport = transport;

    private readonly ITickProcessor processor = processor;

    private readonly CancellationTokenSource stopping = new();

    private Task? executionTask;

    public Task StartAsync(CancellationToken ct)
    {
        executionTask = processor.ProcessAsync(transport.ReadAllAsync(stopping.Token), stopping.Token).AsTask();

        return executionTask.IsCompleted
            ? executionTask
            : Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken ct)
    {
        if (executionTask == null)
        {
            return;
        }

        var completed = await Task.WhenAny(
            executionTask,
            Task.Delay(Timeout.Infinite, ct));

        if (completed != executionTask)
        {
            await stopping.CancelAsync();
        }

        await executionTask;
    }

    public void Dispose()
    {
        stopping.Dispose();
    }
}
