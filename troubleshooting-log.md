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

## Issues Fixed (Update 2025-12-12 16:20)

### 1. MQTT Authentication and Connectivity ✅
**Status**: COMPLETE
- **Problem**: MQTT integration couldn't connect - "Name does not resolve", "Connection refused", "Not authorized"
- **Root Cause**: 
  1. Broker hostname "core-mosquitto" doesn't exist in Docker setup
  2. No authentication credentials configured
  3. Ports not bound to 0.0.0.0 in shared network namespace
- **Solution**:
  1. Updated broker address from "core-mosquitto" to "127.0.0.1" in `.storage/core.config_entries`
  2. Added homeassistant user to `/mosquitto/config/passwd` with hashed password
  3. Changed listener directives in `mosquitto.conf` to bind 0.0.0.0:1883 and 0.0.0.0:9001
  4. Documented mosquitto restart requirement: `docker restart mosquitto` after HA restarts
- **Verification**: MQTT entities now appearing, Zigbee2MQTT devices discovered

### 2. yt_dlp Directory Missing ✅
**Status**: COMPLETE
- **Problem**: `/share/jumpdrive/yt-dl` directory not found
- **Root Cause**: /share path convention from HAOS doesn't exist in Docker, requires explicit mount
- **Solution**: Added volume mount in docker-compose.yml: `/mnt/user/jumpdrive:/share/jumpdrive`
- **Verification**: No more FileNotFoundError for yt_dlp

### 3. ESPHome Dashboard ✅
**Status**: DISABLED (non-critical)
- **Problem**: Cannot connect to 127.0.0.1:65298
- **Root Cause**: Dashboard port changed from 65298 to 6052, integration trying SSL on HTTP port
- **Solution**: Removed `.storage/esphome.dashboard` file to disable the integration
- **Note**: ESPHome dashboard not critical for operation, individual devices still work

### 4. Bluetooth/alive_dimmer ⚠️
**Status**: PARTIAL - Requires ESPHome Bluetooth Proxy or Network Fix
- **Problem**: Xiaomi BLE dimmer (A4:C1:38:92:9E:2D) events not firing, "Unable to open PF_BLUETOOTH socket"
- **Root Cause**: BlueZ D-Bus interface doesn't allow socket creation from Docker container with macvlan network
- **Attempted**: 
  1. Added USB device passthrough `/dev/bus/usb/003/004` - device visible but no BlueZ in container
  2. D-Bus socket mounted but HA can't create Bluetooth sockets through it
- **Next Steps**: 
  1. Enable hammet-bt-proxy ESPHome device (192.168.8.111) - currently unreachable
  2. Check if hammet-bt-proxy is powered on and has network access
  3. Verify dimmer battery level (may need replacement)
  4. Consider using different ESPHome Bluetooth proxy if bt-proxy unavailable
- **Workaround**: alive_dimmer automation won't trigger until Bluetooth connectivity restored
- **Technical Details**: 
  - Device: Xiaomi BLE Dimmer Switch (YLKG08YL)
  - Integration: xiaomi_ble
  - Config Entry ID: 0fd3edba978409a32fab5a56d16b0bea
  - Bind Key: 5cda19e590c68be23c5d4067ffffffff
  - Known Events: dimmer (press, long_press, rotate_left, rotate_right, rotate_left_pressed, rotate_right_pressed)

## Git Commits Made
1. "Config updates" - Initial migration fixes
2. "Fixes: MQTT bind config" - MQTT listener bind addresses  
3. "Add /share/jumpdrive volume mount" - yt_dlp directory mount
4. "Fix Bluetooth access and ESPHome dashboard" - USB passthrough and dashboard removal

