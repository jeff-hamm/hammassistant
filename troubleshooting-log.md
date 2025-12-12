# Home Assistant Troubleshooting Log
Date: 2025-12-12

## Actions Taken

### 1. Removed tuya_ble component (REVERTED)
- **Action**: Deleted `/config/custom_components/tuya_ble`
- **Reason**: ImportError: cannot import name 'CONF_APP_TYPE'
- **Status**: REVERTED - Component restored from `/root/appdata/tmp/custom_components/tuya_ble`
- **Note**: User wants to keep this component. Error indicates compatibility issue with current HA version.

### 2. Moved ingress.yaml (REVERTED)
- **Action**: Renamed `config/yaml/global/ingress.yaml` to `ingress.yaml.bak`
- **Reason**: Thought it was causing configuration errors
- **Status**: REVERTED - File restored to original location
- **Note**: Configuration was actually correct.

### 3. Git commit
- **Action**: Committed outstanding changes
- **Commit message**: "Config updates: ingress merge, refresh-config script, docker compose improvements"
- **Status**: COMPLETED
- **Files committed**: 
  - .gitignore updates
  - copilot-instructions.md updates
  - docker-compose.yml improvements
  - refresh-config.ps1 (new PowerShell script)
  - Various config files

## Outstanding Issues to Address

### Critical Errors
1. **tuya_ble ImportError**
   - Error: `cannot import name 'CONF_APP_TYPE' from 'homeassistant.components.tuya.const'`
   - Impact: Component fails to load
   - Solution needed: Update tuya_ble to work with current HA version or temporarily disable

2. **MQTT Connection Failure**
   - Error: `Failed to connect to MQTT server due to exception: [Errno -2] Name does not resolve`
   - Impact: Shellies automations fail, MQTT unavailable
   - Solution needed: Check mosquitto container DNS resolution

3. **yt_dlp Missing Directory**
   - Error: `FileNotFoundError: [Errno 2] No such file or directory: '/share/jumpdrive/yt-dl'`
   - Solution needed: Create missing directory

4. **o365 Configuration Error**
   - Error: `KeyError: 'accounts'`
   - Solution needed: Fix o365 configuration or disable integration

5. **aidot Configuration Error**
   - Error: `KeyError: 'login_info'`
   - Solution needed: Fix aidot configuration or disable integration

### Warnings
1. **Google Assistant 403 Errors**
   - Error: 403 responses from homegraph.googleapis.com
   - Impact: Google Assistant sync failing
   - Solution needed: Check credentials/permissions

2. **Template Variable Errors**
   - Error: `'event_name' is undefined`
   - Location: automation.smart_light_test
   - Solution needed: Fix template in automation

3. **ESPHome Dashboard Connection**
   - Error: Cannot connect to 127.0.0.1:65298
   - Impact: ESPHome dashboard unavailable
   - Solution needed: Check ESPHome container/addon configuration

## Actions Log (Continued)

### 4. Tuya BLE Handling (Updated Approach)
- **Action**: Disabled tuya_ble by renaming to `tuya_ble.disabled`
- **Reason**: Component incompatible with current HA version (missing CONF_APP_TYPE)
- **Status**: DISABLED - Can be re-enabled when updated version available
- **Backup**: Component saved in `custom_components/tuya_ble.backup`

## Next Steps
- Restart HA to see current errors without tuya_ble
- Fix configuration-based issues only
- Don't modify external component code
- Test after each restart
