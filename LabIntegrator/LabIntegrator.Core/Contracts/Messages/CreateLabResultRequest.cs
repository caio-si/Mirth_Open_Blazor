using System.ComponentModel.DataAnnotations;

namespace LabIntegrator.Core.Contracts.Messages;

/// <summary>
/// Dados enviados junto à mensagem para persistir resultados laboratoriais.
/// </summary>
public class CreateLabResultRequest
{
    /// <summary>
    /// Identificador do paciente na mensagem de origem.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PatientId { get; set; } = string.Empty;

    /// <summary>
    /// Nome do paciente.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Código do exame.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TestCode { get; set; } = string.Empty;

    /// <summary>
    /// Descrição opcional do exame.
    /// </summary>
    [MaxLength(200)]
    public string? TestDescription { get; set; }

    /// <summary>
    /// Valor do resultado.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Unidade do resultado.
    /// </summary>
    [MaxLength(20)]
    public string? Units { get; set; }

    /// <summary>
    /// Faixa de referência opcional.
    /// </summary>
    [MaxLength(100)]
    public string? ReferenceRange { get; set; }

    /// <summary>
    /// Data/hora de liberação do resultado; padrão: atual.
    /// </summary>
    public DateTime? ResultedAt { get; set; }
}



