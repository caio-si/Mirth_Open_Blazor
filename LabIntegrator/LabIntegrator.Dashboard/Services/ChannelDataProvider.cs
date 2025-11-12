using LabIntegrator.Core.Contracts.Channels;

namespace LabIntegrator.Dashboard.Services;

/// <summary>
/// Encapsula o consumo de canais para ser usado pelos componentes.
/// </summary>
public class ChannelDataProvider
{
    private readonly ApiClient _apiClient;

    public ChannelDataProvider(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyCollection<ChannelDto>> GetChannelsAsync(CancellationToken cancellationToken = default)
        => _apiClient.GetChannelsAsync(cancellationToken);
}



