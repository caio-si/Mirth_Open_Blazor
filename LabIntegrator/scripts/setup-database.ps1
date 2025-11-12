param(
    [string]$Configuration = "Development",
    [string]$SolutionRoot = (Resolve-Path "..")
)

Write-Host "==> Aplicando migrações do Entity Framework Core ($Configuration)..." -ForegroundColor Cyan

$env:ASPNETCORE_ENVIRONMENT = $Configuration

Push-Location $SolutionRoot

try {
    dotnet restore | Out-Null
    dotnet-ef database update `
        --project LabIntegrator.Infrastructure `
        --startup-project LabIntegrator.Api `
        --context LabIntegrator.Infrastructure.Data.LabIntegratorDbContext

    Write-Host "Migrações aplicadas com sucesso." -ForegroundColor Green
}
finally {
    Pop-Location
}

