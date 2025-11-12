using LabIntegrator.Dashboard.Components;
using LabIntegrator.Dashboard.Options;
using LabIntegrator.Dashboard.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configurações tipadas da API de integração.
builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

// HttpClient centralizado que injeta cabeçalhos/padrões.
builder.Services.AddHttpClient(ApiClient.NamedClient, (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<ApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
    client.DefaultRequestHeaders.Add("X-Api-Key", options.ApiKey);
});

// Serviços de dados reaproveitados pelos componentes.
builder.Services.AddTransient<ApiClient>();
builder.Services.AddTransient<ChannelDataProvider>();
builder.Services.AddTransient<MessageDataProvider>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
