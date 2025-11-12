using System.ComponentModel.DataAnnotations;

namespace LabIntegrator.Core.Contracts.Channels;

/// <summary>
/// Modelo enviado pelo cliente para cadastrar um novo canal de integração.
/// </summary>
public class CreateChannelRequest
{
    /// <summary>
    /// Nome do canal, obrigatório e único dentro do contexto.
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição opcional para facilitar a identificação no dashboard.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Indica se o canal já deve iniciar ativo.
    /// </summary>
    public bool IsActive { get; set; } = true;
}

