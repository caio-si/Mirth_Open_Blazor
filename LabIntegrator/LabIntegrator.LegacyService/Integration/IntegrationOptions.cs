namespace LabIntegrator.LegacyService.Integration;

/// <summary>
/// Representa as configurações lidas do appsettings para controlar a simulação do legado.
/// </summary>
public class IntegrationOptions
{
    /// <summary>
    /// Diretório monitorado onde os arquivos de mensagens serão depositados.
    /// </summary>
    public string InputDirectory { get; set; } = @"C:\Integrations\Inbound";

    /// <summary>
    /// Diretório para onde os arquivos processados com sucesso serão movidos.
    /// </summary>
    public string ArchiveDirectory { get; set; } = @"C:\Integrations\Archive";

    /// <summary>
    /// Diretório onde arquivos com erro serão armazenados para auditoria.
    /// </summary>
    public string ErrorDirectory { get; set; } = @"C:\Integrations\Error";

    /// <summary>
    /// Intervalo (segundos) entre varreduras do diretório.
    /// </summary>
    public int PollingIntervalSeconds { get; set; } = 10;

    /// <summary>
    /// URL base da API moderna para envio das mensagens.
    /// </summary>
    public string ApiBaseUrl { get; set; } = "https://localhost:5001";

    /// <summary>
    /// Chave de autenticação (quando necessário) para a API.
    /// </summary>
    public string ApiKey { get; set; } = "local-dev-key";
}



