namespace LabIntegrator.Core.Enums;

/// <summary>
/// Define o estágio do processamento de uma mensagem dentro do pipeline de integração.
/// </summary>
public enum MessageStatus
{
    /// <summary>
    /// Mensagem recém-chegada, ainda não validada.
    /// </summary>
    Received = 0,

    /// <summary>
    /// Mensagem verificada e pronta para transformação.
    /// </summary>
    Validated = 1,

    /// <summary>
    /// Mensagem tratada com sucesso e entregue ao destino.
    /// </summary>
    Processed = 2,

    /// <summary>
    /// Ocorreram erros durante o processamento.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Mensagem armazenada para fins históricos e retirada do fluxo ativo.
    /// </summary>
    Archived = 4
}

