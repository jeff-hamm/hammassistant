param(
    [string]$HomeAssistantUrl = "https://hammlet.infinitebutts.com",
    [string]$Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJmNDExYmVlZTY5MjI0NGE3YTMwZDgxNjcyMDYwYzY5YiIsImlhdCI6MTc2NTQxMzc2MCwiZXhwIjoyMDgwNzczNzYwfQ.wYt-QQyVtNCxUm9CPf0uUW95na5Dt4mGbYgwPT-yXN8"
)

# Prepare API request
$headers = @{
    "Authorization" = "Bearer $Token"
    "Content-Type" = "application/json"
}

$uri = "$HomeAssistantUrl/api/services/automation/reload"

Write-Host "Reloading all automations from configuration files..."

try {
    $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers
    Write-Host "✓ Automations reloaded successfully!" -ForegroundColor Green
    return $response
}
catch {
    Write-Error "Failed to reload automations: $_"
    exit 1
}
