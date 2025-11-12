param(
    [string]$BaseUrl = "https://localhost:5001",
    [string]$ApiKey = "local-dev-key"
)

Write-Host "==> Enviando canais exemplo para $BaseUrl ..." -ForegroundColor Cyan

[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }

$headers = @{ "X-Api-Key" = $ApiKey }

$channels = @(
    @{
        name        = "Hematologia - Sysmex"
        description = "Canal responsável por integrar resultados do equipamento Sysmex"
        isActive    = $true
    },
    @{
        name        = "Bioquímica - Cobas"
        description = "Resultados bioquímicos do analisador Roche Cobas"
        isActive    = $true
    }
)

$channelIds = @()

foreach ($channel in $channels) {
    $response = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/channels" -Headers $headers -Body ($channel | ConvertTo-Json) -ContentType "application/json"
    $channelIds += $response.id
    Write-Host " - Canal criado: $($response.name) (Id: $($response.id))" -ForegroundColor Green
}

$sampleMessage = @{
    channelId        = $channelIds[0]
    externalId       = "MSG-" + [Guid]::NewGuid().ToString("N").Substring(0, 8)
    type             = 2 # Result
    status           = 0 # Received
    payloadRaw       = "{ ""example"": ""payload"" }"
    payloadNormalized= "{ ""patientId"": ""123"" }"
    results          = @(
        @{
            patientId       = "P001"
            patientName     = "João da Silva"
            testCode        = "HB"
            testDescription = "Hemoglobina"
            value           = "13.4"
            units           = "g/dL"
            referenceRange  = "13 - 17"
        }
    )
}

$messageResponse = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/messages" -Headers $headers -Body ($sampleMessage | ConvertTo-Json -Depth 5) -ContentType "application/json"
Write-Host "==> Mensagem exemplo criada (Id: $($messageResponse.id))." -ForegroundColor Green

Write-Host "Seed concluído." -ForegroundColor Cyan



