using FluentAssertions;
using LabIntegrator.Core.Contracts.Messages;
using LabIntegrator.Core.Entities;
using LabIntegrator.Core.Enums;
using LabIntegrator.Infrastructure.Data;
using LabIntegrator.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LabIntegrator.Tests.Services;

/// <summary>
/// Testes de integração focados em cenários de hematologia (equipamentos Mindray),
/// garantindo que a ingestão de resultados esteja consistente.
/// </summary>
public class LabMessageServiceTests
{
    /// <summary>
    /// Verifica se uma mensagem Mindray é persistida com seus resultados.
    /// </summary>
    [Fact(DisplayName = "Hematologia Mindray - persistência de mensagem e resultados")]
    public async Task CreateMindrayResult_ShouldPersistMessageAndResults()
    {
        await using var context = CreateDbContext();
        var channel = await SeedMindrayChannelAsync(context);

        var service = new LabMessageService(context, NullLogger<LabMessageService>.Instance);

        var request = new CreateMessageRequest
        {
            ChannelId = channel.Id,
            ExternalId = "MINDRAY-HEM-001",
            Type = MessageType.Result,
            Status = MessageStatus.Received,
            PayloadRaw = "{ \"device\":\"MindrayBC-6200\",\"type\":\"hematology\" }",
            Results = new List<CreateLabResultRequest>
            {
                new()
                {
                    PatientId = "H12345",
                    PatientName = "Paciente Mindray",
                    TestCode = "WBC",
                    TestDescription = "Contagem de leucócitos",
                    Value = "7.2",
                    Units = "10^3/µL",
                    ReferenceRange = "4.0 - 10.0"
                }
            }
        };

        var messageDto = await service.CreateAsync(request, CancellationToken.None);

        messageDto.Should().NotBeNull();
        messageDto.Results.Should().HaveCount(1);
        messageDto.ExternalId.Should().Be("MINDRAY-HEM-001");

        var persisted = await context.Messages.Include(m => m.Results).SingleAsync();
        persisted.Results.Should().ContainSingle(r => r.TestCode == "WBC" && r.Value == "7.2");
    }

    /// <summary>
    /// Garante que mensagens duplicadas do equipamento Mindray sejam rejeitadas.
    /// </summary>
    [Fact(DisplayName = "Hematologia Mindray - rejeição de duplicidade por ExternalId")]
    public async Task CreateMindrayResult_DuplicateExternalId_ShouldThrow()
    {
        await using var context = CreateDbContext();
        var channel = await SeedMindrayChannelAsync(context);

        var service = new LabMessageService(context, NullLogger<LabMessageService>.Instance);

        var request = new CreateMessageRequest
        {
            ChannelId = channel.Id,
            ExternalId = "MINDRAY-HEM-002",
            Type = MessageType.Result,
            Status = MessageStatus.Received,
            PayloadRaw = "{ \"device\":\"MindrayBC-6200\",\"type\":\"hematology\" }"
        };

        await service.CreateAsync(request, CancellationToken.None);

        var act = async () => await service.CreateAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*ExternalId*");
    }

    private static LabIntegratorDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<LabIntegratorDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new LabIntegratorDbContext(options);
    }

    private static async Task<LabChannel> SeedMindrayChannelAsync(LabIntegratorDbContext context)
    {
        var channel = new LabChannel
        {
            Name = "Hematologia - Mindray BC-6200",
            Description = "Integração Mindray para contagem completa de células",
            IsActive = true
        };

        context.Channels.Add(channel);
        await context.SaveChangesAsync();

        return channel;
    }
}

