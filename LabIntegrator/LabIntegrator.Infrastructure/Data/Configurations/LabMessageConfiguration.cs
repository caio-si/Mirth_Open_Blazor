using LabIntegrator.Core.Entities;
using LabIntegrator.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LabIntegrator.Infrastructure.Data.Configurations;

/// <summary>
/// Configura o mapeamento da entidade <see cref="LabMessage"/> com as colunas,
/// conversões e índices necessários para performance e integridade.
/// </summary>
public class LabMessageConfiguration : IEntityTypeConfiguration<LabMessage>
{
    /// <summary>
    /// Define como a entidade deve ser persistida no banco.
    /// </summary>
    public void Configure(EntityTypeBuilder<LabMessage> builder)
    {
        builder.ToTable("LabMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PayloadRaw)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .HasDefaultValue(MessageType.Unknown);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .HasDefaultValue(MessageStatus.Received);

        builder.Property(x => x.ReceivedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.ChannelId, x.ExternalId })
            .IsUnique();

        builder.HasMany(x => x.Results)
            .WithOne(x => x.Message!)
            .HasForeignKey(x => x.MessageId);
    }
}

