#!/bin/bash
# Add sensitive secrets to GitHub repository
# Usage: ./add-secrets.sh

export GH_CONFIG_DIR=$PHOME/.auth/gh
GH=$PHOME/.local/bin/gh
REPO=jeff-hamm/hammassistant

echo "Adding sensitive secrets to GitHub repository $REPO"
echo ""

# Read CLOUDFLARE_TUNNEL_TOKEN
read -sp "Enter CLOUDFLARE_TUNNEL_TOKEN (from Cloudflare Zero Trust dashboard): " CLOUDFLARE_TOKEN
echo ""
if [ -n "$CLOUDFLARE_TOKEN" ]; then
    echo "$CLOUDFLARE_TOKEN" | $GH secret set CLOUDFLARE_TUNNEL_TOKEN --repo $REPO
    echo "✓ Set CLOUDFLARE_TUNNEL_TOKEN"
fi

# Read HA_TOKEN
read -sp "Enter HA_TOKEN (Home Assistant Long-Lived Access Token): " HA_TOKEN_VAL
echo ""
if [ -n "$HA_TOKEN_VAL" ]; then
    echo "$HA_TOKEN_VAL" | $GH secret set HA_TOKEN --repo $REPO
    echo "✓ Set HA_TOKEN"
fi

echo ""
echo "Done! All secrets added to GitHub."
echo ""
echo "Secrets stored:"
echo "  - Environment: TZ, COMPOSE_ROOT, NETWORK_*, CLOUDFLARE_TUNNEL_TOKEN, HA_TOKEN"
echo "  - MQTT: MOSQUITTO_SYSTEM_USER (mosquitto/data/system_user.json)"
echo "  - ZwaveJS: ZWAVEJS_USERS, ZWAVEJS_SETTINGS (zwavejs/store/*.json)"
echo "  - Matter: MATTER_CREDENTIALS (75+ PEM certificates as tar.gz)"
echo ""
echo "To list all secrets:"
echo "  export GH_CONFIG_DIR=$PHOME/.auth/gh"
echo "  $GH secret list --repo $REPO"
echo ""
echo "To restore secrets locally:"
echo "  See scripts/restore-secrets.sh"
