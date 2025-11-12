using LabIntegrator.LegacyService;
using LabIntegrator.LegacyService.Integration;

var builder = Host.CreateApplicationBuilder(args);

// Configurações do serviço legado (diretórios, API, etc.).
builder.Services.Configure<IntegrationOptions>(
    builder.Configuration.GetSection("Integration"));

// HttpClient configurado para chamar a API moderna.
builder.Services.AddHttpClient("integration-api", client =>
{
    var options = builder.Configuration.GetSection("Integration").Get<IntegrationOptions>() ?? new IntegrationOptions();
    client.BaseAddress = new Uri(options.ApiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("X-Api-Key", options.ApiKey);
});

builder.Services.AddSingleton<IntegrationService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
