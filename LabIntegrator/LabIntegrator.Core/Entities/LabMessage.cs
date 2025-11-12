using LabIntegrator.Core.Enums;

namespace LabIntegrator.Core.Entities;

/// <summary>
/// Representa uma mensagem recebida do equipamento laboratoriais ou sistemas externos.
/// Guarda o payload bruto, a versão normalizada e o estado de processamento.
/// </summary>
public class LabMessage
{
    /// <summary>
    /// Identificador único da mensagem.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Chave estrangeira que aponta para o canal responsável pela mensagem.
    /// </summary>
    public Guid ChannelId { get; set; }

    /// <summary>
    /// Identificador externo fornecido pelo equipamento/fonte de dados.
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da mensagem para dirigir a orquestração (ex.: pedido, resultado).
    /// </summary>
    public MessageType Type { get; set; } = MessageType.Unknown;

    /// <summary>
    /// Status atual do processamento da mensagem.
    /// </summary>
    public MessageStatus Status { get; set; } = MessageStatus.Received;

    /// <summary>
    /// Conteúdo original recebido, preservado para auditoria e reprocessamento.
    /// </summary>
    public string PayloadRaw { get; set; } = string.Empty;

    /// <summary>
    /// Versão normalizada (JSON/HL7 parseado) utilizada pelo restante do sistema.
    /// </summary>
    public string? PayloadNormalized { get; set; }

    /// <summary>
    /// Data/hora de recebimento da mensagem.
    /// </summary>
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data/hora em que o processamento foi concluído com sucesso.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Detalhes do último erro, quando o processamento falha.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Navegação para o canal associado.
    /// </summary>
    public LabChannel? Channel { get; set; }

    /// <summary>
    /// Resultados laboratoriais derivados desta mensagem.
    /// </summary>
    public ICollection<LabResult> Results { get; set; } = new List<LabResult>();
}

