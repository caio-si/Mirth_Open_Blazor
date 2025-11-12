namespace LabIntegrator.Core.Enums;

/// <summary>
/// Define os tipos de mensagens aceitas pelo motor de integração.
/// Usado para roteamento e aplicação de regras específicas de processamento.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Categoria padrão aplicada quando o tipo não é identificado.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Mensagens de solicitação de exames enviadas por sistemas clínicos/ERP.
    /// </summary>
    Order = 1,

    /// <summary>
    /// Mensagens de resultado liberado pelos equipamentos laboratoriais.
    /// </summary>
    Result = 2,

    /// <summary>
    /// Confirmações de recebimento (ACK) utilizadas para reconciliação.
    /// </summary>
    Acknowledgement = 3,

    /// <summary>
    /// Eventos técnicos emitidos pelos instrumentos (status, alarmes, etc.).
    /// </summary>
    InstrumentEvent = 4
}

