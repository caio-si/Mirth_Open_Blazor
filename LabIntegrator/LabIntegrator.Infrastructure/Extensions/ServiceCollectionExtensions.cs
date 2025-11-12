using LabIntegrator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LabIntegrator.Infrastructure.Extensions;

/// <summary>
/// Conjunto de extensões de serviço que encapsulam a configuração da camada de infraestrutura.
/// Permite registrar o DbContext a partir de qualquer host (API, worker, testes).
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra dependências de infraestrutura, iniciando pelo DbContext configurado via connection string.
    /// </summary>
    /// <param name="services">Coleção de serviços do host.</param>
    /// <param name="configuration">Fonte de configuração para leitura das connection strings.</param>
    /// <returns>Coleção de serviços enriquecida, permitindo encadeamento.</returns>
    /// <exception cref="InvalidOperationException">Lançada quando a connection string não está definida.</exception>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Default' was not found.");
        }

        services.AddDbContext<LabIntegratorDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}

