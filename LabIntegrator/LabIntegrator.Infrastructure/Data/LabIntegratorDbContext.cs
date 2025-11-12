using LabIntegrator.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabIntegrator.Infrastructure.Data;

/// <summary>
/// DbContext central responsável por compor o modelo relacional da aplicação.
/// É utilizado pela API, serviço legado e testes para manipular o banco SQL Server.
/// </summary>
public class LabIntegratorDbContext(DbContextOptions<LabIntegratorDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Conjunto de canais cadastrados.
    /// </summary>
    public DbSet<LabChannel> Channels => Set<LabChannel>();

    /// <summary>
    /// Conjunto de mensagens recebidas.
    /// </summary>
    public DbSet<LabMessage> Messages => Set<LabMessage>();

    /// <summary>
    /// Conjunto de resultados laboratoriais detalhados.
    /// </summary>
    public DbSet<LabResult> Results => Set<LabResult>();

    /// <summary>
    /// Aplica as configurações de mapeamento localizadas no assembly de infraestrutura.
    /// Mantém o mapeamento desacoplado das entidades.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LabIntegratorDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

