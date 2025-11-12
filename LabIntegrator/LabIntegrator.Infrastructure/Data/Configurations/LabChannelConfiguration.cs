using LabIntegrator.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LabIntegrator.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração fluent para o entity <see cref="LabChannel"/>, centralizando regras
/// de mapeamento (tabela, índices e relacionamentos).
/// </summary>
public class LabChannelConfiguration : IEntityTypeConfiguration<LabChannel>
{
    /// <summary>
    /// Aplica restrições de schema para garantir consistência no banco.
    /// </summary>
    public void Configure(EntityTypeBuilder<LabChannel> builder)
    {
        builder.ToTable("LabChannels");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Channel)
            .HasForeignKey(x => x.ChannelId);
    }
}

