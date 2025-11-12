namespace LabIntegrator.Core.Entities;

/// <summary>
/// Representa um canal de integração configurado pelo time para receber mensagens laboratoriais.
/// Mantém metadados administrativos utilizados pela API e pelo serviço legado.
/// </summary>
public class LabChannel
{
    /// <summary>
    /// Identificador único do canal, utilizado como chave primária e referência externa.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nome amigável exibido no dashboard e usado em relatórios.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição opcional para contextualizar a finalidade do canal (ex.: equipamento ou rotina).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Flag que sinaliza se o canal está ativo para processamento no backend.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Data de criação do registro para auditoria e ordenação.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data da última atualização de configuração, se houver.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Coleção navegável das mensagens que chegaram por este canal.
    /// </summary>
    public ICollection<LabMessage> Messages { get; set; } = new List<LabMessage>();
}

