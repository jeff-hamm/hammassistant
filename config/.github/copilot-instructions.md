# Home Assistant Configuration Context

## File Structure
- **Root**: `/root/hammassistant/` - Home Assistant config directory
- **YAML configs**: `/root/hammassistant/yaml/`
  - `automations/` - Automation definitions (organized by category)
  - `scripts/` - Reusable script definitions
  - `components/` - Helper entities (input_datetime, input_boolean, etc.)
  - `dashboards/` - Dashboard configurations
  - `global/` - Global settings
- **Custom components**: `/root/hammassistant/custom_components/`
- **ESPHome**: `/root/hammassistant/esphome/`

## API Access
- **URLs**: `http://192.168.1.216:8123` (local) and `https://hammlet.infinitebutts.com` (remote)
- **Credentials**: Stored in `/root/hammassistant/yaml/secret.env`
  - `HOME_ASSISTANT_URLS` - Comma-separated failover URLs
  - `HOME_ASSISTANT_TOKEN` - Long-lived access token

### REST API Endpoints
- `/api/services/{domain}/{service}` - POST to call services
- `/api/states/{entity_id}` - GET entity state
- `/api/template` - POST to render Jinja2 templates
- `/api/` - Health check (returns 200 when reachable)

### Common API Patterns
```bash
curl -X POST \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"entity_id": "light.example"}' \
  "http://192.168.1.216:8123/api/services/light/turn_on"
```

## Configuration Management

### Reloading Configuration
**Always use `/root/hammassistant/yaml/refresh-config.sh` after modifying configurations.**

Usage:
```bash
# Reload specific config types
./refresh-config.sh automation
./refresh-config.sh automation script

# Reload all configs
./refresh-config.sh all

# Reload and commit changes to git
./refresh-config.sh automation -m "Updated transition_late automation"
```

**CRITICAL**: After making any changes to automations, scripts, scenes, or components:
1. Run `./refresh-config.sh <config_type>` to reload
2. Use `-m "commit message"` to automatically commit changes
3. Validate the reload was successful (script exits with error on failure)

### Validation
- The refresh script automatically validates by checking HTTP response codes
- Failed reloads will exit with non-zero status
- Check Home Assistant logs at `/root/hammassistant/home-assistant.log` for detailed errors

## Key Custom Scripts

### Light Fading Scripts
- `script.target_lights_fader` - Wrapper that expands target selectors (label_id, area_id, device_id, entity_id)
- `script.ashley_s_light_fader` - Core fading logic with easing functions
- `script.get_target_lights` - Converts target selectors to entity lists

**Target Selector Format**:
```yaml
lights:
  label_id: "turn_off_sleep"     # Target all entities with label
  # OR entity_id: "light.example"  # Target specific entity
  # OR area_id: "bedroom"          # Target all lights in area
  # OR device_id: "abc123"         # Target device
```

**Important**: When passing parameters between scripts, pre-compute values before template rendering to avoid dict-to-string conversion issues.

## Common Helper Patterns

### Input DateTime Helpers
Create in `/root/hammassistant/yaml/components/`:
```yaml
input_datetime:
  my_time_helper:
    name: My Time
    has_date: false
    has_time: true
    initial: '22:30:00'
```

### Time-based Triggers
```yaml
triggers:
- trigger: time
  at: input_datetime.my_time_helper
  id: my_trigger
```

### Weekday Conditions
```yaml
conditions:
- condition: time
  weekday:
  - mon
  - tue
  - wed
  - thu
```

## Environment Notes
- Host: Linux (jumpdrive)
- Python: 3.11 available for testing/scripting
- Git: Repository at `/root/hammassistant/`
- Home Assistant runs in container, config mounted to `/root/hammassistant/`