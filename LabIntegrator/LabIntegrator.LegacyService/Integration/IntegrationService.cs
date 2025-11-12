using System.Text.Json;
using LabIntegrator.Core.Contracts.Messages;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LabIntegrator.LegacyService.Integration;

/// <summary>
/// Serviço responsável por monitorar a pasta legada e enviar mensagens para a API.
/// </summary>
public class IntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IntegrationOptions _options;
    private readonly ILogger<IntegrationService> _logger;

    /// <summary>
    /// Construtor padrão.
    /// </summary>
    public IntegrationService(
        IHttpClientFactory httpClientFactory,
        IOptions<IntegrationOptions> options,
        ILogger<IntegrationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Garante que os diretórios existam e retorna uma lista de arquivos a processar.
    /// </summary>
    public IEnumerable<string> EnumeratePendingFiles()
    {
        Directory.CreateDirectory(_options.InputDirectory);
        Directory.CreateDirectory(_options.ArchiveDirectory);
        Directory.CreateDirectory(_options.ErrorDirectory);

        return Directory.EnumerateFiles(_options.InputDirectory, "*.json");
    }

    /// <summary>
    /// Processa um arquivo, enviando-o para a API e movendo o arquivo conforme sucesso/erro.
    /// </summary>
    public async Task ProcessFileAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var payload = await File.ReadAllTextAsync(filePath, cancellationToken);
            var request = JsonSerializer.Deserialize<CreateMessageRequest>(payload, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (request is null)
            {
                throw new InvalidOperationException("Não foi possível desserializar a mensagem.");
            }

            var client = _httpClientFactory.CreateClient("integration-api");

            var response = await client.PostAsJsonAsync("/api/messages", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException($"Falha ao enviar mensagem. Status: {response.StatusCode}. Erro: {error}");
            }

            await MoveFileAsync(filePath, _options.ArchiveDirectory, cancellationToken);
            _logger.LogInformation("Arquivo {File} enviado e arquivado com sucesso.", Path.GetFileName(filePath));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar arquivo {File}.", Path.GetFileName(filePath));
            await MoveFileAsync(filePath, _options.ErrorDirectory, cancellationToken);
        }
    }

    private static async Task MoveFileAsync(string sourcePath, string destinationDirectory, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(sourcePath);
        var destinationPath = Path.Combine(destinationDirectory, $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{fileName}");

        await using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.None);
        await using var destinationStream = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await sourceStream.CopyToAsync(destinationStream, cancellationToken);

        File.Delete(sourcePath);
    }
}

