#!/bin/bash
# Restore secrets from GitHub repository
# Usage: ./restore-secrets.sh

export GH_CONFIG_DIR=$PHOME/.auth/gh
GH=$PHOME/.local/bin/gh
REPO=jeff-hamm/hammassistant
REPO_ROOT=/root/appdata/hammassistant

echo "Restoring secrets from GitHub repository $REPO"
echo ""

cd $REPO_ROOT

# Restore .env file
echo "Restoring .env file..."
cat > .env << 'EOF'
# Home Assistant Docker Environment Variables
# Restored from GitHub Secrets

# Cloudflare Tunnel Token
EOF
echo "CLOUDFLARE_TUNNEL_TOKEN=$($GH secret list --repo $REPO --json name,updatedAt | jq -r '.[] | select(.name=="CLOUDFLARE_TUNNEL_TOKEN") | .name' && echo '# See GitHub Secrets')" >> .env
echo "" >> .env
echo "# Home Assistant Long-Lived Access Token" >> .env
echo "HA_TOKEN=# See GitHub Secrets" >> .env
echo "" >> .env
$GH secret list --repo $REPO --json name -q '.[] | select(.name | startswith("TZ") or startswith("COMPOSE") or startswith("NETWORK")) | "export \(.name)" ' | while read line; do
    SECRET_NAME=$(echo $line | cut -d' ' -f2)
    echo "$SECRET_NAME=\$($GH secret list --repo $REPO --json name | jq -r '.[] | select(.name==\"$SECRET_NAME\") | .name')" >> .env
done

echo "⚠ Note: GitHub Actions secrets cannot be read back. You'll need to set these manually:"
echo "  - CLOUDFLARE_TUNNEL_TOKEN"
echo "  - HA_TOKEN"
echo ""

# Restore MQTT credentials
echo "Restoring mosquitto/data/system_user.json..."
mkdir -p mosquitto/data
# Note: Can't retrieve secret value via CLI, need to use in GitHub Actions
echo "⚠ Run this in GitHub Actions to restore: gh secret list outputs base64, decode and save"

# Restore ZwaveJS credentials
echo "Restoring zwavejs/store credentials..."
mkdir -p zwavejs/store
echo "⚠ Run this in GitHub Actions to restore ZWAVEJS_USERS and ZWAVEJS_SETTINGS"

# Restore Matter credentials
echo "Restoring matter-server/credentials..."
mkdir -p matter-server/credentials
echo "⚠ Run this in GitHub Actions to restore MATTER_CREDENTIALS (tar.gz)"

echo ""
echo "============================================================"
echo "SECRET RESTORATION INSTRUCTIONS"
echo "============================================================"
echo ""
echo "GitHub CLI cannot read secret values for security reasons."
echo "To restore secrets, create a GitHub Actions workflow:"
echo ""
cat << 'WORKFLOW'
name: Restore Secrets
on: workflow_dispatch
jobs:
  restore:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Restore MQTT credentials
        run: |
          mkdir -p mosquitto/data
          echo "${{ secrets.MOSQUITTO_SYSTEM_USER }}" | base64 -d > mosquitto/data/system_user.json
      - name: Restore ZwaveJS credentials
        run: |
          mkdir -p zwavejs/store
          echo "${{ secrets.ZWAVEJS_USERS }}" | base64 -d > zwavejs/store/users.json
          echo "${{ secrets.ZWAVEJS_SETTINGS }}" | base64 -d > zwavejs/store/settings.json
      - name: Restore Matter credentials
        run: |
          mkdir -p matter-server/credentials
          echo "${{ secrets.MATTER_CREDENTIALS }}" | base64 -d > /tmp/matter.tar.gz
          tar -xzf /tmp/matter.tar.gz -C .
      - name: Create .env
        run: |
          cat > .env << EOF
          CLOUDFLARE_TUNNEL_TOKEN=${{ secrets.CLOUDFLARE_TUNNEL_TOKEN }}
          HA_TOKEN=${{ secrets.HA_TOKEN }}
          TZ=${{ secrets.TZ }}
          COMPOSE_ROOT=${{ secrets.COMPOSE_ROOT }}
          NETWORK_NAME=${{ secrets.NETWORK_NAME }}
          NETWORK_SUBNET=${{ secrets.NETWORK_SUBNET }}
          NETWORK_GATEWAY=${{ secrets.NETWORK_GATEWAY }}
          NETWORK_MAC=${{ secrets.NETWORK_MAC }}
          EOF
WORKFLOW

echo ""
echo "Save this as .github/workflows/restore-secrets.yml"
