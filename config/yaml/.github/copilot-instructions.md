# Copilot Instructions for Home Assistant YAML Workspace

## Refreshing Configuration

When making changes to configuration files on Windows, automatically refresh them to Home Assistant by running:

```powershell
# Reload all YAML configuration (default)
.\refresh-config.ps1

# Reload specific configuration types
.\refresh-config.ps1 -ConfigTypes automation
.\refresh-config.ps1 -ConfigTypes script
.\refresh-config.ps1 -ConfigTypes scene
.\refresh-config.ps1 -ConfigTypes template

# Reload multiple configuration types (up to 3)
.\refresh-config.ps1 -ConfigTypes automation,script
.\refresh-config.ps1 -ConfigTypes automation,script,scene

# More than 3 types automatically uses reload_all
.\refresh-config.ps1 -ConfigTypes automation,script,scene,template
```

This script reloads configuration from files on the Home Assistant instance at https://hammlet.infinitebutts.com (with fallback to local IP).

**Supported configuration types:**
- `automation` - Automations
- `script` - Scripts
- `scene` - Scenes
- `template` - Template entities
- `theme` - Themes
- `input_boolean`, `input_number`, `input_select`, `input_text`, `input_datetime`, `input_button` - Input helpers
- `group` - Groups
- `rest_command` - REST commands

**When to refresh:**
- After editing files in `automations/` → reload `automation`
- After editing files in `scripts/` → reload `script`
- After editing files in `global/scene.yaml` → reload `scene`
- After editing files in `global/template.yaml` → reload `template`
- After editing multiple types → reload all affected types

**Note:** Only run this on Windows systems. The script requires PowerShell and uses a pre-configured authentication token.
