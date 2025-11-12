namespace LabIntegrator.Core.Contracts.Channels;

/// <summary>
/// Representa os dados expostos nos endpoints e no dashboard para um canal.
/// </summary>
public class ChannelDto
{
    /// <summary>
    /// Identificador único do canal.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Nome amigável configurado pelo operador.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Descrição opcional explicando o objetivo do canal.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Indica se o canal está ativo para processamento.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Quantidade de mensagens associadas, útil para dashboards.
    /// </summary>
    public int TotalMessages { get; init; }

    /// <summary>
    /// Data de criação do canal.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}



