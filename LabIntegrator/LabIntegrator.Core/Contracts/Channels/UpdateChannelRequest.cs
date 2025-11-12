using System.ComponentModel.DataAnnotations;

namespace LabIntegrator.Core.Contracts.Channels;

/// <summary>
/// Modelo utilizado para atualização parcial do canal existente.
/// </summary>
public class UpdateChannelRequest
{
    /// <summary>
    /// Nome atualizado do canal.
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada ou notas operacionais.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Flag que indica se o canal permanece ativo.
    /// </summary>
    public bool IsActive { get; set; } = true;
}

