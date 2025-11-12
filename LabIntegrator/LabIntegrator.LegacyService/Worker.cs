using LabIntegrator.LegacyService.Integration;
using Microsoft.Extensions.Options;

namespace LabIntegrator.LegacyService;

/// <summary>
/// Worker que simula o comportamento do serviço legado monitorando arquivos em uma pasta.
/// </summary>
public class Worker : BackgroundService
{
    private readonly IntegrationService _integrationService;
    private readonly IntegrationOptions _options;
    private readonly ILogger<Worker> _logger;

    /// <summary>
    /// Construtor com injeção das dependências necessárias.
    /// </summary>
    public Worker(
        IntegrationService integrationService,
    IOptions<IntegrationOptions> options,
        ILogger<Worker> logger)
    {
        _integrationService = integrationService;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Loop principal que monitora o diretório legado.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço legado iniciado. Monitorando {Directory}.", _options.InputDirectory);

        while (!stoppingToken.IsCancellationRequested)
        {
            var files = _integrationService.EnumeratePendingFiles().ToList();

            if (files.Count == 0)
            {
                _logger.LogDebug("Nenhum arquivo encontrado. Aguardando {Seconds}s.", _options.PollingIntervalSeconds);
            }

            foreach (var file in files)
            {
                stoppingToken.ThrowIfCancellationRequested();
                await _integrationService.ProcessFileAsync(file, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.PollingIntervalSeconds), stoppingToken);
        }

        _logger.LogInformation("Serviço legado finalizado.");
    }
}
