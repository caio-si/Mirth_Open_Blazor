using LabIntegrator.Core.Interfaces;
using LabIntegrator.Infrastructure.Extensions;
using LabIntegrator.Infrastructure.Services;
using Microsoft.OpenApi.Models;

// Programa principal da API de integração laboratorial. Responsável por montar o host,
// registrar serviços (incluindo infraestrutura) e configurar middlewares.
var builder = WebApplication.CreateBuilder(args);

// Camada de apresentação baseada em controllers para facilitar versionamento e testes.
builder.Services.AddControllers();

// Serviços auxiliares para geração de documentação interativa.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Lab Integrator API",
        Version = "v1",
        Description = "API para integração com equipamentos laboratoriais",
    });
});

// Registro da infraestrutura (DbContext e dependências) com base na configuration atual.
builder.Services.AddInfrastructure(builder.Configuration);

// Serviços de domínio/aplicação expostos para os controladores.
builder.Services.AddScoped<ILabChannelService, LabChannelService>();
builder.Services.AddScoped<ILabMessageService, LabMessageService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
