using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LabIntegrator.Infrastructure.Data;

/// <summary>
/// Implementação de <see cref="IDesignTimeDbContextFactory{TContext}"/> para suportar comandos do EF Core.
/// Garante que as migrações possam ser geradas fora do runtime principal (CLI, ferramentas externas).
/// </summary>
public class LabIntegratorDbContextFactory : IDesignTimeDbContextFactory<LabIntegratorDbContext>
{
    /// <summary>
    /// Cria uma instância do DbContext utilizando a mesma configuração da aplicação.
    /// </summary>
    public LabIntegratorDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("Default")
            ?? "Server=localhost;Database=LabIntegratorDev;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<LabIntegratorDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new LabIntegratorDbContext(optionsBuilder.Options);
    }
}

