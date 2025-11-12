using LabIntegrator.Core.Contracts.Channels;

namespace LabIntegrator.Core.Interfaces;

/// <summary>
/// Define operações relacionadas à gestão de canais de integração.
/// </summary>
public interface ILabChannelService
{
    /// <summary>
    /// Retorna todos os canais cadastrados, com contagem de mensagens.
    /// </summary>
    Task<IReadOnlyCollection<ChannelDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um canal específico ou <c>null</c> caso não exista.
    /// </summary>
    Task<ChannelDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo canal com base no request recebido.
    /// </summary>
    Task<ChannelDto> CreateAsync(CreateChannelRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um canal existente.
    /// </summary>
    Task<ChannelDto?> UpdateAsync(Guid id, UpdateChannelRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um canal e suas mensagens associadas.
    /// </summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}



