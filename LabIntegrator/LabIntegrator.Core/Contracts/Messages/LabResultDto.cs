using LabIntegrator.Core.Enums;

namespace LabIntegrator.Core.Contracts.Messages;

/// <summary>
/// Resultados laboratoriais retornados pelos endpoints de mensagens.
/// </summary>
public class LabResultDto
{
    /// <summary>
    /// Identificador do resultado.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Identificador do paciente.
    /// </summary>
    public string PatientId { get; init; } = string.Empty;

    /// <summary>
    /// Nome do paciente.
    /// </summary>
    public string PatientName { get; init; } = string.Empty;

    /// <summary>
    /// Código do exame.
    /// </summary>
    public string TestCode { get; init; } = string.Empty;

    /// <summary>
    /// Descrição opcional do exame.
    /// </summary>
    public string? TestDescription { get; init; }

    /// <summary>
    /// Valor medido.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Unidade do valor medido.
    /// </summary>
    public string? Units { get; init; }

    /// <summary>
    /// Faixa de referência fornecida pelo equipamento.
    /// </summary>
    public string? ReferenceRange { get; init; }

    /// <summary>
    /// Data/hora da liberação do resultado.
    /// </summary>
    public DateTime ResultedAt { get; init; }
}



