# Copilot Instructions for Home Assistant YAML Workspace

## Refreshing Automations

When making changes to automation files on Windows, automatically refresh them to Home Assistant by running:

```powershell
.\refresh-automation.ps1
```

This script reloads all automations from the configuration files on the Home Assistant instance at https://hammlet.infinitebutts.com.

**When to refresh:**
- After editing any files in `automations/`
- After creating new automation files
- After modifying automation-related scripts in `scripts/`

**Note:** Only run this on Windows systems. The script requires PowerShell and uses a pre-configured authentication token.
