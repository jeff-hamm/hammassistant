param(
    [ArgumentCompleter({
        param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameters)
        $configTypes = @(
            'all', 'automation', 'script', 'scene', 'template', 'theme',
            'input_boolean', 'input_number', 'input_select', 'input_text', 
            'input_datetime', 'input_button', 'group', 'rest_command',
            'timer', 'counter', 'zone', 'person', 'tag', 'schedule'
        )
        $configTypes | Where-Object { $_ -like "$wordToComplete*" } | ForEach-Object { $_ }
    })]
    [string[]]$ConfigTypes,
    [string[]]$HomeAssistantUrls,
    [string]$Token
)

# Known Home Assistant configuration types
$knownConfigTypes = @(
    'automation', 'script', 'scene', 'template', 'theme',
    'input_boolean', 'input_number', 'input_select', 'input_text', 
    'input_datetime', 'input_button', 'group', 'rest_command',
    'timer', 'counter', 'zone', 'person', 'tag', 'schedule'
)

# Load environment variables from secret.env if parameters not provided
if (-not $HomeAssistantUrls -or -not $Token) {
    $envFile = Join-Path $PSScriptRoot "secret.env"
    if (Test-Path $envFile) {
        try {
            Import-Module pwsh-dotenv -ErrorAction Stop
            Import-Dotenv -Path $envFile
            
            if (-not $HomeAssistantUrls) {
                $HomeAssistantUrls = $env:HOME_ASSISTANT_URLS -split ','
            }
            if (-not $Token) {
                $Token = $env:HOME_ASSISTANT_TOKEN
            }
        }
        catch {
            Write-Error "Failed to load secret.env. Install pwsh-dotenv module: Install-Module pwsh-dotenv"
            exit 1
        }
    }
    else {
        Write-Error "secret.env file not found and required parameters not provided"
        exit 1
    }
}

# Prepare API request
$headers = @{
    "Authorization" = "Bearer $Token"
    "Content-Type" = "application/json"
}

# Try to find a working URL
$baseUrl = $null
foreach ($url in $HomeAssistantUrls) {
    Write-Host "Trying $url..." -ForegroundColor Yellow
    
    try {
        $testUri = "$url/api/"
        $response = Invoke-RestMethod -Uri $testUri -Method Get -Headers $headers -TimeoutSec 5
        Write-Host "✓ Connected to $url" -ForegroundColor Green
        $baseUrl = $url
        break
    }
    catch {
        Write-Host "✗ Failed to connect to $url" -ForegroundColor Red
    }
}

if (-not $baseUrl) {
    Write-Error "Failed to connect to any Home Assistant URLs"
    exit 1
}

# Validate and process config types
if ($ConfigTypes) {
    # Check if 'all' is specified
    if ($ConfigTypes -contains 'all') {
        $ConfigTypes = $null  # Treat as reload all
    }
    else {
        # Validate config types
        foreach ($type in $ConfigTypes) {
            if ($type -notin $knownConfigTypes) {
                Write-Warning "Unknown configuration type: $type (will attempt to reload anyway)"
            }
        }
    }
}

# If no config types specified or more than 3, reload all YAML config
if (-not $ConfigTypes -or $ConfigTypes.Count -gt 3) {
    $uri = "$baseUrl/api/services/homeassistant/reload_all"
    Write-Host "`nReloading all YAML configuration..."
    
    try {
        $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers
        Write-Host "✓ All YAML configuration reloaded successfully!" -ForegroundColor Green
        exit 0
    }
    catch {
        Write-Error "Failed to reload all configuration: $_"
        exit 1
    }
}

# Reload each specific configuration type
$allSuccess = $true
foreach ($configType in $ConfigTypes) {
    $uri = "$baseUrl/api/services/$configType/reload"
    Write-Host "`nReloading $configType from configuration files..."
    
    try {
        $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers
        Write-Host "✓ $configType reloaded successfully!" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Failed to reload $configType : $_" -ForegroundColor Red
        $allSuccess = $false
    }
}

if (-not $allSuccess) {
    Write-Error "Some configuration types failed to reload"
    exit 1
}
