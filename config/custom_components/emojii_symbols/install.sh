#!/usr/bin/env bash
set -euo pipefail

# Installs svgmoji emoji SVGs + aliases into this custom component.
#
# The `data/` directory is intentionally generated and can be excluded from git.
#
# Usage:
#   ./install.sh
#   ./install.sh --emoji-set twemoji
#   ./install.sh --emoji-set noto
#   ./install.sh --ref main

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

if ! command -v python3 >/dev/null 2>&1; then
  echo "python3 is required" >&2
  exit 1
fi

EMOJI_SET="twemoji"
REF="main"
CLEAN="--clean"

while [[ $# -gt 0 ]]; do
  case "$1" in
    --emoji-set)
      EMOJI_SET="$2"; shift 2 ;;
    --ref)
      REF="$2"; shift 2 ;;
    --no-clean)
      CLEAN=""; shift ;;
    -h|--help)
      sed -n '1,80p' "$0"; exit 0 ;;
    *)
      echo "Unknown argument: $1" >&2
      exit 2
      ;;
  esac
done

echo "Installing svgmoji set: $EMOJI_SET (ref: $REF)"
python3 tools/sync_svgmoji.py --mode all --source github --ref "$REF" --emoji-set "$EMOJI_SET" $CLEAN

echo "Done. Icons should now be available as emo:<alias> or emo:<CODEPOINT>."
