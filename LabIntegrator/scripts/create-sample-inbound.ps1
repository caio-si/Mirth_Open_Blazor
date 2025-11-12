param(
    [string]$InboundDirectory = "C:\Integrations\Inbound",
    [Guid]$ChannelId,
    [string]$ExternalId,
    [string]$PatientId = "P001",
    [string]$PatientName = "João da Silva"
)

if (-not $ExternalId) {
    $ExternalId = "MSG-" + [Guid]::NewGuid().ToString("N").Substring(0, 8)
}

Write-Host "==> Gerando arquivo de mensagem em $InboundDirectory" -ForegroundColor Cyan

New-Item -ItemType Directory -Path $InboundDirectory -Force | Out-Null

$payload = @{
    channelId        = $ChannelId
    externalId       = $ExternalId
    type             = 2 # Result
    status           = 0
    payloadRaw       = "{ ""legacyMessage"": ""Conteúdo bruto fictício"" }"
    results          = @(
        @{
            patientId       = $PatientId
            patientName     = $PatientName
            testCode        = "HB"
            testDescription = "Hemoglobina"
            value           = "13.4"
            units           = "g/dL"
            referenceRange  = "13 - 17"
        }
    )
}

$fileName = Join-Path $InboundDirectory "$ExternalId.json"
$payload | ConvertTo-Json -Depth 5 | Set-Content -Path $fileName -Encoding UTF8

Write-Host "Arquivo criado: $fileName" -ForegroundColor Green



