using LabIntegrator.Core.Contracts.Channels;
using LabIntegrator.Core.Entities;

namespace LabIntegrator.Infrastructure.Mappings;

/// <summary>
/// Métodos auxiliares para conversão entre entidades e DTOs de canal.
/// Mantém o mapeamento centralizado e reutilizável.
/// </summary>
public static class ChannelMappings
{
    /// <summary>
    /// Converte uma entidade <see cref="LabChannel"/> para <see cref="ChannelDto"/>.
    /// </summary>
    public static ChannelDto ToDto(this LabChannel entity, int totalMessages = 0)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            TotalMessages = totalMessages
        };

    /// <summary>
    /// Aplica os valores de <see cref="CreateChannelRequest"/> em uma nova entidade.
    /// </summary>
    public static LabChannel ToEntity(this CreateChannelRequest request)
        => new()
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsActive = request.IsActive
        };

    /// <summary>
    /// Sobrepõe os dados de um canal existente a partir de <see cref="UpdateChannelRequest"/>.
    /// </summary>
    public static void ApplyUpdate(this LabChannel entity, UpdateChannelRequest request)
    {
        entity.Name = request.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
    }
}



