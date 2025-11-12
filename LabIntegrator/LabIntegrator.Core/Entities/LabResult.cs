namespace LabIntegrator.Core.Entities;

/// <summary>
/// Representa um resultado laboratorial extraído de uma mensagem processada.
/// Estrutura as informações usadas pelos sistemas consumidores e pelo dashboard.
/// </summary>
public class LabResult
{
    /// <summary>
    /// Identificador primário do resultado.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Referência para a mensagem da qual o resultado foi derivado.
    /// </summary>
    public Guid MessageId { get; set; }

    /// <summary>
    /// Identificador do paciente conforme enviado pelo equipamento/sistema externo.
    /// </summary>
    public string PatientId { get; set; } = string.Empty;

    /// <summary>
    /// Nome do paciente, para facilitar auditoria humana.
    /// </summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Código do exame/teste conforme catálogo do laboratório.
    /// </summary>
    public string TestCode { get; set; } = string.Empty;

    /// <summary>
    /// Descrição opcional e legível do exame.
    /// </summary>
    public string? TestDescription { get; set; }

    /// <summary>
    /// Valor resultado do exame.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Unidade de medida associada ao valor.
    /// </summary>
    public string? Units { get; set; }

    /// <summary>
    /// Faixa de referência informada pelo sistema de origem.
    /// </summary>
    public string? ReferenceRange { get; set; }

    /// <summary>
    /// Data/hora em que o exame foi liberado pelo laboratório.
    /// </summary>
    public DateTime ResultedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navegação para a mensagem original.
    /// </summary>
    public LabMessage? Message { get; set; }
}

