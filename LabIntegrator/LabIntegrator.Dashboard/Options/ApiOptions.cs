namespace LabIntegrator.Dashboard.Options;

/// <summary>
/// Configurações de acesso à API de integração utilizadas no dashboard.
/// </summary>
public class ApiOptions
{
    public const string SectionName = "Api";

    /// <summary>
    /// URL base da API back-end.
    /// </summary>
    public string BaseUrl { get; set; } = "https://localhost:5001";

    /// <summary>
    /// Chave de acesso utilizada para autenticação simples (se aplicável).
    /// </summary>
    public string ApiKey { get; set; } = "local-dev-key";
}



