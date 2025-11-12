using LabIntegrator.Core.Contracts.Messages;
using LabIntegrator.Core.Enums;
using LabIntegrator.Core.Interfaces;
using LabIntegrator.Infrastructure.Data;
using LabIntegrator.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LabIntegrator.Infrastructure.Services;

/// <summary>
/// Serviço responsável por orquestrar a ingestão e consulta de mensagens via EF Core.
/// </summary>
public class LabMessageService : ILabMessageService
{
    private readonly LabIntegratorDbContext _dbContext;
    private readonly ILogger<LabMessageService> _logger;

    /// <summary>
    /// Construtor padrão com injeção das dependências necessárias.
    /// </summary>
    public LabMessageService(LabIntegratorDbContext dbContext, ILogger<LabMessageService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<MessageDto>> GetRecentAsync(Guid? channelId = null, int take = 50, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Messages
            .AsNoTracking()
            .Include(message => message.Results)
            .OrderByDescending(message => message.ReceivedAt)
            .Where(message => channelId == null || message.ChannelId == channelId)
            .Take(Math.Clamp(take, 1, 200));

        var entities = await query.ToListAsync(cancellationToken);
        return entities.Select(entity => entity.ToDto()).ToList();
    }

    /// <inheritdoc />
    public async Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Messages
            .AsNoTracking()
            .Include(message => message.Results)
            .SingleOrDefaultAsync(message => message.Id == id, cancellationToken);

        return entity?.ToDto();
    }

    /// <inheritdoc />
    public async Task<MessageDto> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Messages
            .AnyAsync(message =>
                message.ChannelId == request.ChannelId &&
                message.ExternalId == request.ExternalId,
                cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Já existe uma mensagem com o mesmo ExternalId para este canal.");
        }

        var entity = request.ToEntity();

        _dbContext.Messages.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Mensagem {MessageId} registrada para o canal {ChannelId}.", entity.Id, entity.ChannelId);

        return entity.ToDto();
    }

    /// <inheritdoc />
    public async Task<bool> UpdateStatusAsync(Guid id, string? errorMessage, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Messages
            .SingleOrDefaultAsync(message => message.Id == id, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        entity.ErrorMessage = errorMessage;
        entity.Status = string.IsNullOrWhiteSpace(errorMessage)
            ? MessageStatus.Processed
            : MessageStatus.Failed;
        entity.ProcessedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Mensagem {MessageId} teve status atualizado para {Status}.",
            entity.Id,
            entity.Status);

        return true;
    }
}



