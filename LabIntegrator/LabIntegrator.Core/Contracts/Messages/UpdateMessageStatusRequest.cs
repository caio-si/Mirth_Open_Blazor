using System.ComponentModel.DataAnnotations;

namespace LabIntegrator.Core.Contracts.Messages;

/// <summary>
/// Requisição para atualizar o status de processamento de uma mensagem.
/// </summary>
public class UpdateMessageStatusRequest
{
    /// <summary>
    /// Quando verdadeiro, indica sucesso e limpa mensagens de erro.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Mensagem de erro opcional para registrar causa de falha.
    /// Obrigatória quando <see cref="Succeeded"/> for falso.
    /// </summary>
    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Valida que o erro foi preenchido em caso de falha.
    /// </summary>
    public bool IsValid()
    {
        return Succeeded || !string.IsNullOrWhiteSpace(ErrorMessage);
    }
}



