# LabIntegrator

Projeto de estudo para simular um middleware de integração laboratorial em C#. A solução foi estruturada para demonstrar manutenção de legado, criação de uma API moderna, camada de dados com EF Core e um dashboard para monitoramento.

## Estrutura da solução

- `LabIntegrator.Core`  
  Contém entidades de domínio, DTOs e contratos de serviço compartilhados.
- `LabIntegrator.Infrastructure`  
  Persistência com Entity Framework Core, mapeamentos fluent e migrações.
- `LabIntegrator.Api`  
  API ASP.NET Core com endpoints REST para canais e mensagens, já integrada ao EF Core e documentada via Swagger.
- `LabIntegrator.LegacyService`  
  Worker Service que simula o sistema legado: monitora uma pasta de entrada, lê arquivos JSON e envia mensagens para a API.
- `LabIntegrator.Dashboard`  
  Dashboard Blazor Server que consome a API e exibe visão geral de canais e mensagens recentes.
- `LabIntegrator.Tests`  
  Projeto reservado para testes automatizados (ponto de evolução).

## Configuração inicial

1. Ajuste a connection string em `LabIntegrator.Api/appsettings.Development.json` (padrão apontando para SQL Server local).  
2. Aplique as migrações:
   ```powershell
   cd .\LabIntegrator\scripts
   .\setup-database.ps1
   ```
3. (Opcional) Monte as pastas do legado caso utilize caminhos diferentes dos padrões (`C:\Integrations\Inbound`, `Archive`, `Error`).

## Execução dos serviços

Em terminais separados:

```powershell
cd .\LabIntegrator
dotnet run --project LabIntegrator.Api

dotnet run --project LabIntegrator.LegacyService

dotnet run --project LabIntegrator.Dashboard
```

> O dashboard será exposto em `https://localhost:7003` (porta padrão do template) e consome a API em `https://localhost:5001`. Ajuste `appsettings.json` caso altere as portas.

## Scripts auxiliares (`/scripts`)

- `setup-database.ps1` – aplica as migrações do EF Core usando o projeto da API como startup.  
- `seed-sample-data.ps1` – envia canais e uma mensagem de exemplo diretamente para a API (exige API em execução).  
- `create-sample-inbound.ps1` – gera um arquivo JSON no diretório monitorado pelo serviço legado, forçando a ingestão via Worker.

Exemplo de uso:

```powershell
.\seed-sample-data.ps1 -BaseUrl https://localhost:5001

.\create-sample-inbound.ps1 -ChannelId "<guid do canal>" -InboundDirectory "C:\Integrations\Inbound"
```

## Testes

Execute todos os testes (a solution já compila sem erros e está pronta para receber cenários automatizados):

```powershell
cd .\LabIntegrator
dotnet test
```

## Observações

- Garantir que o serviço de API esteja rodando antes de executar o Worker legado ou o dashboard.  
- Os scripts ignoram validação de certificado apenas para facilitar o ambiente local (autoassinado). Ajuste conforme políticas internas.  
- O dashboard consome diretamente os endpoints para exibir métricas em tempo real; ampliar com SignalR é um passo natural se necessário.  

