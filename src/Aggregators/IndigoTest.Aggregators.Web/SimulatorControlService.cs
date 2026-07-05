using System.Text.Json;
using System.Text.Json.Serialization;
using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Sources;
using IndigoTest.Simulators.Common.Faults;
using IndigoTest.Simulators.Common.Status;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Web;

internal sealed class SimulatorControlService(
    IHttpClientFactory clients,
    IOptions<AggregatorOptions> options)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter(),
        },
    };

    private readonly IHttpClientFactory clients = clients;

    private readonly AggregatorOptions options = options.Value;

    public async Task<IReadOnlyList<SimulatorDashboardSnapshot>> GetSnapshotsAsync(CancellationToken ct)
    {
        var tasks = options.Sources
            .Select(source => GetSnapshotAsync(source, ct))
            .ToArray();

        return await Task.WhenAll(tasks);
    }

    public async Task<SimulatorDashboardSnapshot?> UpdateStateAsync(
        string sourceName,
        SimulatorState state,
        CancellationToken ct)
    {
        var source = options.Sources.FirstOrDefault(x => string.Equals(x.Source, sourceName, StringComparison.Ordinal));
        if (source == null)
        {
            return null;
        }

        var client = clients.CreateClient(nameof(SimulatorControlService));
        var response = await client.PostAsJsonAsync(GetStateUrl(source), state, JsonOptions, ct);

        response.EnsureSuccessStatusCode();

        return await GetSnapshotAsync(source, ct);
    }

    private async Task<SimulatorDashboardSnapshot> GetSnapshotAsync(SourceDefinition source, CancellationToken ct)
    {
        try
        {
            var client = clients.CreateClient(nameof(SimulatorControlService));
            var status = await client.GetFromJsonAsync<SimulatorStatusSnapshot>(GetStatusUrl(source), JsonOptions, ct);

            if (status == null)
            {
                return CreateUnavailable(source, "Empty response.");
            }

            return new SimulatorDashboardSnapshot
            {
                Source = source.Source,
                Kind = source.Kind,
                Url = source.Url,
                IsAvailable = true,
                ActiveConnections = status.ActiveConnections,
                TotalGeneratedTicks = status.TotalGeneratedTicks,
                TotalSentMessages = status.TotalSentMessages,
                TotalDuplicateMessages = status.TotalDuplicateMessages,
                LastSymbol = status.LastSymbol,
                LastSentAt = status.LastSentAt,
                IsUnavailable = status.IsUnavailable,
                UnavailableUntil = status.UnavailableUntil,
                State = status.State,
            };
        }
        catch (Exception exception)
        {
            return CreateUnavailable(source, exception.Message);
        }
    }

    private static SimulatorDashboardSnapshot CreateUnavailable(SourceDefinition source, string error)
    {
        return new SimulatorDashboardSnapshot
        {
            Source = source.Source,
            Kind = source.Kind,
            Url = source.Url,
            IsAvailable = false,
            Error = error,
        };
    }

    private static string GetStatusUrl(SourceDefinition source)
    {
        return CreateHttpUri(source.Url, "/api/status");
    }

    private static string GetStateUrl(SourceDefinition source)
    {
        return CreateHttpUri(source.Url, "/api/state");
    }

    private static string CreateHttpUri(string webSocketUrl, string path)
    {
        var builder = new UriBuilder(webSocketUrl)
        {
            Scheme = webSocketUrl.StartsWith("wss://", StringComparison.OrdinalIgnoreCase)
                ? Uri.UriSchemeHttps
                : Uri.UriSchemeHttp,
            Path = path,
            Query = string.Empty,
        };

        return builder.Uri.ToString();
    }
}
