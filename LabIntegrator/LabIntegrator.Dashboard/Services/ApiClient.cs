using System.Net.Http.Json;
using LabIntegrator.Core.Contracts.Channels;
using LabIntegrator.Core.Contracts.Messages;
using LabIntegrator.Dashboard.Options;
using Microsoft.Extensions.Options;

namespace LabIntegrator.Dashboard.Services;

/// <summary>
/// Cliente HTTP reaproveitado pelos data providers do dashboard.
/// </summary>
public class ApiClient
{
    public const string NamedClient = "dashboard-api";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiOptions _options;
    private readonly ILogger<ApiClient> _logger;

    public ApiClient(
        IHttpClientFactory httpClientFactory,
        IOptions<ApiOptions> options,
        ILogger<ApiClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient(NamedClient);

    public async Task<IReadOnlyCollection<ChannelDto>> GetChannelsAsync(CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await client.GetAsync("/api/channels", cancellationToken);
        await EnsureSuccessAsync(response);

        var payload = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ChannelDto>>(cancellationToken: cancellationToken);
        return payload ?? Array.Empty<ChannelDto>();
    }

    public async Task<IReadOnlyCollection<MessageDto>> GetMessagesAsync(Guid? channelId = null, int take = 50, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var query = channelId.HasValue
            ? $"/api/messages?channelId={channelId}&take={take}"
            : $"/api/messages?take={take}";

        var response = await client.GetAsync(query, cancellationToken);
        await EnsureSuccessAsync(response);

        var payload = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<MessageDto>>(cancellationToken: cancellationToken);
        return payload ?? Array.Empty<MessageDto>();
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Falha ao consumir API {Status}: {Error}", response.StatusCode, error);
        response.EnsureSuccessStatusCode();
    }
}



