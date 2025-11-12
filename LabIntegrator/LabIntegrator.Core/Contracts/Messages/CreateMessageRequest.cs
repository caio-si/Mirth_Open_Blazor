using System.ComponentModel.DataAnnotations;
using LabIntegrator.Core.Enums;

namespace LabIntegrator.Core.Contracts.Messages;

/// <summary>
/// Representa a requisição de ingestão de uma nova mensagem laboratorial.
/// </summary>
public class CreateMessageRequest
{
    /// <summary>
    /// Canal responsável pela mensagem.
    /// </summary>
    [Required]
    public Guid ChannelId { get; set; }

    /// <summary>
    /// Identificador externo para rastreabilidade.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da mensagem a fim de reger regras de negócio.
    /// </summary>
    [Required]
    public MessageType Type { get; set; } = MessageType.Unknown;

    /// <summary>
    /// Status inicial da mensagem (por padrão, Received).
    /// </summary>
    public MessageStatus Status { get; set; } = MessageStatus.Received;

    /// <summary>
    /// Conteúdo bruto recebido do equipamento (HL7, XML, JSON, etc.).
    /// </summary>
    [Required]
    public string PayloadRaw { get; set; } = string.Empty;

    /// <summary>
    /// Conteúdo normalizado opcional enviado pelo produtor.
    /// </summary>
    public string? PayloadNormalized { get; set; }

    /// <summary>
    /// Resultados laboratoriais extraídos no momento da ingestão.
    /// </summary>
    public ICollection<CreateLabResultRequest> Results { get; set; } = new List<CreateLabResultRequest>();
}



