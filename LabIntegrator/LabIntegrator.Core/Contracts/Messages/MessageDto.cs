using LabIntegrator.Core.Enums;

namespace LabIntegrator.Core.Contracts.Messages;

/// <summary>
/// Representa uma mensagem laboratorial disponibilizada via API.
/// </summary>
public class MessageDto
{
    /// <summary>
    /// Identificador da mensagem.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Canal associado.
    /// </summary>
    public Guid ChannelId { get; init; }

    /// <summary>
    /// Identificador externo fornecido pela origem.
    /// </summary>
    public string ExternalId { get; init; } = string.Empty;

    /// <summary>
    /// Tipo da mensagem (pedido, resultado, etc.).
    /// </summary>
    public MessageType Type { get; init; }

    /// <summary>
    /// Status atual no pipeline de processamento.
    /// </summary>
    public MessageStatus Status { get; init; }

    /// <summary>
    /// Conteúdo bruto armazenado para auditoria.
    /// </summary>
    public string PayloadRaw { get; init; } = string.Empty;

    /// <summary>
    /// Representação normalizada (JSON) para consumo interno.
    /// </summary>
    public string? PayloadNormalized { get; init; }

    /// <summary>
    /// Data em que a mensagem foi recebida.
    /// </summary>
    public DateTime ReceivedAt { get; init; }

    /// <summary>
    /// Data em que a mensagem concluiu processamento.
    /// </summary>
    public DateTime? ProcessedAt { get; init; }

    /// <summary>
    /// Mensagem de erro caso o processamento tenha falhado.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Resultados laboratoriais associados.
    /// </summary>
    public IReadOnlyCollection<LabResultDto> Results { get; init; } = Array.Empty<LabResultDto>();
}



