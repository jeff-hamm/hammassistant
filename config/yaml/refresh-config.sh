#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage:
  refresh-config.sh [CONFIG_TYPE ...] [-m "commit message"]

Examples:
  ./refresh-config.sh automation
  ./refresh-config.sh automation script scene
  ./refresh-config.sh all
  ./refresh-config.sh automation -m "Updated late transition automation"

Options:
  -m MESSAGE    After successful reload, git add and commit with MESSAGE

Configuration:
  - If HOME_ASSISTANT_URLS and HOME_ASSISTANT_TOKEN are not set, this script
    loads them from ./secret.env (same folder as this script).

secret.env format:
  HOME_ASSISTANT_URLS=http://host1:8123,http://host2:8123
  HOME_ASSISTANT_TOKEN=your_long_lived_access_token
EOF
}

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

log_file_default="$script_dir/../home-assistant.log"
HA_LOG_FILE="${HA_LOG_FILE:-$log_file_default}"

die() {
  echo "ERROR: $*" >&2
  exit 1
}

log_line_count() {
  local f="$1"
  if [[ -f "$f" ]]; then
    wc -l < "$f" | tr -d ' '
  else
    echo 0
  fi
}

check_ha_log_for_parse_errors() {
  local config_type="$1"
  local before_lines="$2"

  # Allow opting out if desired
  if [[ "${REFRESH_CONFIG_SKIP_LOG_CHECK:-}" == "1" ]]; then
    return 0
  fi

  if [[ ! -f "$HA_LOG_FILE" ]]; then
    return 0
  fi

  # Give HA a moment to write errors to disk
  sleep "${REFRESH_CONFIG_LOG_SLEEP_S:-0.6}"

  local start_line=$((before_lines + 1))
  local new_log
  new_log="$(tail -n +"$start_line" "$HA_LOG_FILE" 2>/dev/null || true)"
  [[ -n "$new_log" ]] || return 0

  # YAML parse errors often span multiple lines; match on common markers.
  # Example:
  #   while parsing a block mapping
  #   expected <block end>, but found '<block mapping start>'
  local parse_pat='while parsing a block mapping|expected <block end>|mapping values are not allowed|found character that cannot start any token|ScannerError|ParserError|Invalid config for \['

  # Narrow to relevant component where possible, but always catch generic YAML parser errors.
  local component_pat=""
  if [[ "$config_type" != "all" && -n "$config_type" ]]; then
    component_pat="\\[homeassistant\\.components\\.${config_type}\\]"
  fi

  local has_parse_error=false
  if echo "$new_log" | grep -Eqi "$parse_pat"; then
    has_parse_error=true
  fi

  # If component is known and any error line from that component occurred, show it too.
  local component_err=false
  if [[ -n "$component_pat" ]] && echo "$new_log" | grep -Eqi "ERROR .*${component_pat}"; then
    component_err=true
  fi

  if [[ "$has_parse_error" == "true" || "$component_err" == "true" ]]; then
    echo "✗ Home Assistant logged errors during reload ($config_type)." >&2
    echo "--- log excerpt ($HA_LOG_FILE) ---" >&2
    # Print the tail of new log to include multi-line YAML parser context.
    echo "$new_log" | tail -n 80 >&2
    echo "--- end excerpt ---" >&2
    return 1
  fi
}

# Load env from secret.env if needed
if [[ -z "${HOME_ASSISTANT_URLS:-}" || -z "${HOME_ASSISTANT_TOKEN:-}" ]]; then
  env_file="$script_dir/secret.env"
  if [[ -f "$env_file" ]]; then
    set -a
    # shellcheck disable=SC1090
    source "$env_file"
    set +a
  else
    die "secret.env not found and HOME_ASSISTANT_URLS/HOME_ASSISTANT_TOKEN not provided"
  fi
fi

# secret.env may have CRLF; strip any carriage returns.
HOME_ASSISTANT_URLS="${HOME_ASSISTANT_URLS//$'\r'/}"
HOME_ASSISTANT_TOKEN="${HOME_ASSISTANT_TOKEN//$'\r'/}"

IFS=',' read -r -a ha_urls <<< "${HOME_ASSISTANT_URLS:-}"
[[ ${#ha_urls[@]} -gt 0 ]] || die "HOME_ASSISTANT_URLS is empty"
[[ -n "${HOME_ASSISTANT_TOKEN:-}" ]] || die "HOME_ASSISTANT_TOKEN is empty"

auth_header="Authorization: Bearer $HOME_ASSISTANT_TOKEN"
ct_header="Content-Type: application/json"

# Find a working base URL
base_url=""
for url in "${ha_urls[@]}"; do
  # Trim whitespace and trailing slash, tolerate spaces after commas.
  url="${url//$'\r'/}"
  url="$(echo "$url" | xargs)"
  url="${url%/}"
  [[ -n "$url" ]] || continue
  echo "Trying $url..."

  # /api/ should return 200 when reachable; we include the token anyway.
  code="$(curl -sS -o /dev/null -w '%{http_code}' \
    -H "$auth_header" \
    "$url/api/" \
    --max-time 5 || true)"

  if [[ "$code" == "200" ]]; then
    echo "✓ Connected to $url"
    base_url="$url"
    break
  fi

  echo "✗ Failed to connect to $url (HTTP $code)" >&2
done

[[ -n "$base_url" ]] || die "Failed to connect to any Home Assistant URLs"

# Parse args: config types and optional commit message
config_types=()
commit_message=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    -m)
      shift
      commit_message="$1"
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      config_types+=("$1")
      shift
      ;;
  esac
done

commit_if_requested() {
  if [[ -n "$commit_message" ]]; then
    echo
    echo "Committing changes to git..."
    cd "$script_dir/.."
    git add -A
    if git diff --staged --quiet; then
      echo "✓ No changes to commit"
    else
      git commit -m "$commit_message"
      echo "✓ Changes committed: $commit_message"
    fi
  fi
}

# Treat 'all' as reload_all
reload_all=false
for t in "${config_types[@]:-}"; do
  if [[ "$t" == "all" ]]; then
    reload_all=true
    break
  fi
done

if [[ ${#config_types[@]} -eq 0 ]]; then
  reload_all=true
fi

if [[ "$reload_all" == "true" || ${#config_types[@]} -gt 3 ]]; then
  echo
  echo "Reloading all YAML configuration..."
  before_lines="$(log_line_count "$HA_LOG_FILE")"
  code="$(curl -sS -o /dev/null -w '%{http_code}' \
    -X POST \
    -H "$auth_header" -H "$ct_header" \
    -d '{}' \
    "$base_url/api/services/homeassistant/reload_all")"

  [[ "$code" == "200" ]] || die "Failed to reload all configuration (HTTP $code)"
  check_ha_log_for_parse_errors "all" "$before_lines" || die "Home Assistant reported errors during reload_all"
  echo "✓ All YAML configuration reloaded successfully!"
  commit_if_requested
  exit 0
fi

all_success=true
for config_type in "${config_types[@]}"; do
  echo
  echo "Reloading $config_type from configuration files..."

  before_lines="$(log_line_count "$HA_LOG_FILE")"

  code="$(curl -sS -o /dev/null -w '%{http_code}' \
    -X POST \
    -H "$auth_header" -H "$ct_header" \
    -d '{}' \
    "$base_url/api/services/$config_type/reload" || true)"

  if [[ "$code" == "200" ]]; then
    echo "✓ $config_type reloaded successfully!"
    if ! check_ha_log_for_parse_errors "$config_type" "$before_lines"; then
      all_success=false
    fi
  else
    echo "✗ Failed to reload $config_type (HTTP $code)" >&2
    all_success=false
  fi
done

if [[ "$all_success" != "true" ]]; then
  die "Some configuration types failed to reload"
fi

commit_if_requested
