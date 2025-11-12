using LabIntegrator.Core.Contracts.Channels;
using LabIntegrator.Core.Entities;
using LabIntegrator.Core.Interfaces;
using LabIntegrator.Infrastructure.Data;
using LabIntegrator.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LabIntegrator.Infrastructure.Services;

/// <summary>
/// Implementação de <see cref="ILabChannelService"/> baseada em Entity Framework Core.
/// </summary>
public class LabChannelService : ILabChannelService
{
    private readonly LabIntegratorDbContext _dbContext;
    private readonly ILogger<LabChannelService> _logger;

    /// <summary>
    /// Construtor padrão com injeção do DbContext e logger.
    /// </summary>
    public LabChannelService(LabIntegratorDbContext dbContext, ILogger<LabChannelService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ChannelDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var channels = await _dbContext.Channels
            .AsNoTracking()
            .Select(channel => new
            {
                Entity = channel,
                MessageCount = channel.Messages.Count
            })
            .ToListAsync(cancellationToken);

        return channels
            .Select(x => x.Entity.ToDto(x.MessageCount))
            .ToList();
    }

    /// <inheritdoc />
    public async Task<ChannelDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = await _dbContext.Channels
            .AsNoTracking()
            .Where(channel => channel.Id == id)
            .Select(channel => new
            {
                Entity = channel,
                MessageCount = channel.Messages.Count
            })
            .SingleOrDefaultAsync(cancellationToken);

        return query?.Entity.ToDto(query.MessageCount);
    }

    /// <inheritdoc />
    public async Task<ChannelDto> CreateAsync(CreateChannelRequest request, CancellationToken cancellationToken = default)
    {
        var entity = request.ToEntity();

        _dbContext.Channels.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Canal {ChannelId} criado por meio da API.", entity.Id);

        return entity.ToDto();
    }

    /// <inheritdoc />
    public async Task<ChannelDto?> UpdateAsync(Guid id, UpdateChannelRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Channels
            .SingleOrDefaultAsync(channel => channel.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        entity.ApplyUpdate(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Canal {ChannelId} atualizado.", entity.Id);

        return entity.ToDto();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Channels
            .Include(channel => channel.Messages)
            .SingleOrDefaultAsync(channel => channel.Id == id, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        // As mensagens relacionadas serão removidas via cascade.
        _dbContext.Channels.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Canal {ChannelId} removido juntamente com mensagens relacionadas.", entity.Id);

        return true;
    }
}



