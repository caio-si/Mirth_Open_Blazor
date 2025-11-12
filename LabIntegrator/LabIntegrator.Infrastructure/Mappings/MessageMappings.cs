using LabIntegrator.Core.Contracts.Messages;
using LabIntegrator.Core.Entities;

namespace LabIntegrator.Infrastructure.Mappings;

/// <summary>
/// Métodos auxiliares para mapear mensagens e resultados entre entidades e DTOs.
/// </summary>
public static class MessageMappings
{
    /// <summary>
    /// Converte uma entidade <see cref="LabMessage"/> para <see cref="MessageDto"/>.
    /// </summary>
    public static MessageDto ToDto(this LabMessage entity)
    {
        var results = entity.Results?
            .Select(result => new LabResultDto
            {
                Id = result.Id,
                PatientId = result.PatientId,
                PatientName = result.PatientName,
                TestCode = result.TestCode,
                TestDescription = result.TestDescription,
                Value = result.Value,
                Units = result.Units,
                ReferenceRange = result.ReferenceRange,
                ResultedAt = result.ResultedAt
            })
            .ToArray() ?? Array.Empty<LabResultDto>();

        return new MessageDto
        {
            Id = entity.Id,
            ChannelId = entity.ChannelId,
            ExternalId = entity.ExternalId,
            Type = entity.Type,
            Status = entity.Status,
            PayloadRaw = entity.PayloadRaw,
            PayloadNormalized = entity.PayloadNormalized,
            ReceivedAt = entity.ReceivedAt,
            ProcessedAt = entity.ProcessedAt,
            ErrorMessage = entity.ErrorMessage,
            Results = results
        };
    }

    /// <summary>
    /// Constrói uma nova entidade de mensagem a partir da requisição.
    /// </summary>
    public static LabMessage ToEntity(this CreateMessageRequest request)
    {
        var message = new LabMessage
        {
            ChannelId = request.ChannelId,
            ExternalId = request.ExternalId.Trim(),
            Type = request.Type,
            Status = request.Status,
            PayloadRaw = request.PayloadRaw,
            PayloadNormalized = string.IsNullOrWhiteSpace(request.PayloadNormalized)
                ? NormalizePayload(request.PayloadRaw)
                : request.PayloadNormalized,
            ReceivedAt = DateTime.UtcNow
        };

        foreach (var resultRequest in request.Results)
        {
            message.Results.Add(new LabResult
            {
                PatientId = resultRequest.PatientId.Trim(),
                PatientName = resultRequest.PatientName.Trim(),
                TestCode = resultRequest.TestCode.Trim(),
                TestDescription = string.IsNullOrWhiteSpace(resultRequest.TestDescription)
                    ? null
                    : resultRequest.TestDescription.Trim(),
                Value = resultRequest.Value,
                Units = resultRequest.Units,
                ReferenceRange = resultRequest.ReferenceRange,
                ResultedAt = resultRequest.ResultedAt ?? DateTime.UtcNow
            });
        }

        return message;
    }

    private static string NormalizePayload(string payloadRaw)
    {
        // Normalização simples apenas para fins de demonstração.
        return payloadRaw.Trim();
    }
}



