using LabIntegrator.Core.Contracts.Messages;

namespace LabIntegrator.Core.Interfaces;

/// <summary>
/// Define operações para ingestão e consulta de mensagens laboratoriais.
/// </summary>
public interface ILabMessageService
{
    /// <summary>
    /// Lista mensagens recentes, com possibilidade de filtrar por canal.
    /// </summary>
    Task<IReadOnlyCollection<MessageDto>> GetRecentAsync(Guid? channelId = null, int take = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna uma mensagem específica por identificador.
    /// </summary>
    Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste nova mensagem e resultados associados.
    /// </summary>
    Task<MessageDto> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza o status de processamento de uma mensagem.
    /// </summary>
    Task<bool> UpdateStatusAsync(Guid id, string? errorMessage, CancellationToken cancellationToken = default);
}



