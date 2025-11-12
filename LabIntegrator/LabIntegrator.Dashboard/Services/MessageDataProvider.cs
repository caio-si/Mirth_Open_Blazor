using LabIntegrator.Core.Contracts.Messages;

namespace LabIntegrator.Dashboard.Services;

/// <summary>
/// Fornece métodos utilitários para buscar mensagens ordenadas por recência.
/// </summary>
public class MessageDataProvider
{
    private readonly ApiClient _apiClient;

    public MessageDataProvider(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyCollection<MessageDto>> GetMessagesAsync(Guid? channelId = null, int take = 50, CancellationToken cancellationToken = default)
        => _apiClient.GetMessagesAsync(channelId, take, cancellationToken);
}



