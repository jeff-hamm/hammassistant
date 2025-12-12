# Home Assistant Docker Migration

Migrated from HAOS VM (hammassistant) on 2025-12-11

## Directory Structure

```
hammassistant-new/
├── config/                 # Home Assistant /config (byte-for-byte copy)
│   ├── .storage/           # Entity registry, auth, etc.
│   ├── custom_components/  # HACS integrations
│   ├── www/               # Frontend resources
│   ├── yaml/              # YAML packages
│   ├── zigbee2mqtt/       # Zigbee2MQTT config
│   ├── netdaemon5/        # NetDaemon compiled apps
│   └── ...
├── mosquitto/             # MQTT broker
│   ├── config/
│   │   ├── mosquitto.conf
│   │   └── passwd
│   ├── data/
│   └── log/
├── zwavejs/               # Z-Wave JS UI data
│   └── store/
│       ├── settings.json
│       └── *.jsonl        # Z-Wave network data
├── esphome/               # ESPHome configs
├── matter-server/         # Matter Server data
├── netdaemon/             # NetDaemon V5 binaries
├── whisper/               # Whisper voice data
├── piper/                 # Piper TTS data
├── openwakeword/          # Wake word data
├── docker-compose.yml
├── .env                   # Environment variables
└── README.md
```

## Pre-Migration Checklist

- [ ] Stop the HAOS VM: `virsh destroy hammassistant`
- [ ] Verify USB devices are available on host
- [ ] Set `CLOUDFLARE_TUNNEL_TOKEN` in `.env`
- [ ] Generate `HA_TOKEN` in `.env` (after first HA start)

## USB Devices

When the VM is stopped, these devices become available:

| Device | USB ID | Host Path | Container Path |
|--------|--------|-----------|----------------|
| Zooz 800 Z-Wave Stick | 1a86:55d4 | /dev/ttyACM0 | /dev/zwave |
| TP-Link Bluetooth | 2357:0604 | USB passthrough | /dev/bus/usb/... |

**IMPORTANT**: The USB device paths may change. After VM shutdown, run:
```bash
ls -la /dev/serial/by-id/
ls -la /dev/ttyACM*
lsusb
```

## First Start

1. Stop the VM:
   ```bash
   virsh destroy hammassistant
   ```

2. Wait for USB devices to appear, then verify:
   ```bash
   ls -la /dev/ttyACM0
   ```

3. Uncomment USB device mappings in `docker-compose.yml`

4. Start services:
   ```bash
   cd /mnt/pool/appdata/hammassistant-new
   docker compose up -d
   ```

5. Check logs:
   ```bash
   docker compose logs -f homeassistant
   docker compose logs -f zwavejs
   ```

6. Access services:
   - Home Assistant: http://192.168.1.216:8123
   - Z-Wave JS UI: http://192.168.1.216:8091
   - Zigbee2MQTT: http://192.168.1.216:8099
   - ESPHome: http://192.168.1.216:6052

## Configuration Changes Made

### Zigbee2MQTT
- MQTT server changed from `core-mosquitto` to `mosquitto` (Docker service name)
- Serial port remains `tcp://192.168.1.174:6638` (remote coordinator)

### Z-Wave JS UI
- Serial port changed to `/dev/zwave` (mapped from host)
- MQTT host changed from `core-mosquitto` to `mosquitto`
- Config directory updated for Docker paths

### Mosquitto
- New password file created with same `addons` credentials
- Listeners on 1883 (MQTT) and 9001 (WebSocket)

## Migrated Addons

| HAOS Addon | Docker Container | Notes |
|------------|------------------|-------|
| core_mosquitto | mosquitto | Same credentials |
| a0d7b954_zwavejs2mqtt | zwavejs | Full data migrated |
| 45df7312_zigbee2mqtt | zigbee2mqtt | Uses remote coordinator |
| 5c53de3b_esphome | esphome | Network mode: host |
| core_matter_server | matter-server | Data migrated |
| c6a2317c_netdaemon5 | netdaemon | Needs HA_TOKEN |
| 9074a9fa_cloudflared | cloudflared | Needs tunnel token |
| core_whisper | whisper | Voice assistant |
| core_piper | piper | TTS |
| core_openwakeword | openwakeword | Wake word |

## Not Migrated

These HAOS-specific addons don't have direct Docker equivalents:
- **Samba share** - Use Unraid native SMB instead
- **SSH addon** - Use Unraid SSH instead
- **File editor** - Use VS Code or Unraid terminal
- **Portainer** - Already running elsewhere
- **Logspout** - Optional, can add if needed
- **Dnsmasq** - Use router DNS

## Security Keys (IMPORTANT - DO NOT LOSE)

These are stored in `zwavejs/store/settings.json`:

- S2_Unauthenticated: `3A490BC3E89CBEF3E388D0175D2BF5D5`
- S2_Authenticated: `D7EA81C634F530CFE5501DB769645607`
- S2_AccessControl: `A0C0FF5DD5BFE26E29FFC2C422AD7565`
- S0_Legacy: `AD2D24FB3A769F5EFB55475E88FA4509`

Zigbee network key is in `config/zigbee2mqtt/configuration.yaml`

## Cloudflare Tunnel

You'll need to either:
1. Create a new tunnel in Cloudflare Zero Trust and set the token
2. Or migrate the existing tunnel credentials from the addon

Original hostnames configured:
- hammlet.infinitebutts.com (Home Assistant)
- And 17 other services...

## Rollback

The VM is preserved. To rollback:
1. Stop Docker containers: `docker compose down`
2. Start VM: `virsh start hammassistant`
3. USB devices will be re-attached to VM

## Troubleshooting

### Z-Wave not connecting
```bash
# Check if device exists
ls -la /dev/ttyACM0

# Check Z-Wave container logs
docker logs zwavejs

# Verify USB mapping in docker-compose.yml
```

### Home Assistant database errors
The SQLite database may have issues. Check:
```bash
docker exec homeassistant sqlite3 /config/home-assistant_v2.db "PRAGMA integrity_check"
```

### MQTT connection issues
```bash
# Test MQTT connection
docker exec mosquitto mosquitto_sub -u addons -P 'bohyeesahth7JuCoo8Hohcias1deem7gei6Aichai6chie3zaePohfoo1Fohl7xo' -t '#' -v
```
