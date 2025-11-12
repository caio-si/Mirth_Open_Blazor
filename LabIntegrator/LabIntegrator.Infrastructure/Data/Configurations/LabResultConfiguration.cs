using LabIntegrator.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LabIntegrator.Infrastructure.Data.Configurations;

/// <summary>
/// Define o mapeamento relacional da entidade <see cref="LabResult"/>,
/// incluindo restrições, tamanhos de campo e índices utilizados em consultas.
/// </summary>
public class LabResultConfiguration : IEntityTypeConfiguration<LabResult>
{
    /// <summary>
    /// Aplica as configurações de schema necessárias para preservar integridade.
    /// </summary>
    public void Configure(EntityTypeBuilder<LabResult> builder)
    {
        builder.ToTable("LabResults");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PatientId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PatientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.TestCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TestDescription)
            .HasMaxLength(200);

        builder.Property(x => x.Units)
            .HasMaxLength(20);

        builder.Property(x => x.ReferenceRange)
            .HasMaxLength(100);

        builder.Property(x => x.ResultedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.PatientId, x.TestCode, x.ResultedAt });
    }
}

