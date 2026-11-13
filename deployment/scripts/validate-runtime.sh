#!/usr/bin/env bash

set -euo pipefail
umask 077

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
COMPOSE_FILE="$BASE_DIR/deployment/docker-compose.yml"
ENV_TEMPLATE="$BASE_DIR/deployment/.env.example"
ENV_FILE="$BASE_DIR/deployment/.env"

fail() {
  echo "$1" >&2
  exit 1
}

command -v docker >/dev/null 2>&1 || fail "docker is required to validate deployment configuration."
[[ -f "$COMPOSE_FILE" ]] || fail "Missing deployment/docker-compose.yml."
[[ -f "$ENV_TEMPLATE" ]] || fail "Missing deployment/.env.example."

for asset_dir in schema stored-procedures seed; do
  [[ -d "$BASE_DIR/deployment/sql/$asset_dir" ]] || fail "Missing deployment/sql/$asset_dir directory."
done

TEMPLATE_ONLY=false
if [[ -f "$ENV_FILE" ]]; then
  ENV_SOURCE="$ENV_FILE"
else
  ENV_SOURCE="$ENV_TEMPLATE"
  TEMPLATE_ONLY=true
fi

# Compose variables without defaults are required in the selected env file.
required_vars="$(grep -Eo '\$\{[A-Za-z_][A-Za-z0-9_]*\}' "$COMPOSE_FILE" | sed 's/^\${//; s/}$//' | sort -u)"
while IFS= read -r variable; do
  [[ -n "$variable" ]] || continue
  line="$(grep -E "^[[:space:]]*${variable}[[:space:]]*=" "$ENV_SOURCE" | tail -n 1 || true)"
  [[ -n "$line" ]] || fail "Selected env file is missing required Compose variable: $variable."

  value="${line#*=}"
  value="${value%$'\r'}"
  [[ -n "${value//[[:space:]]/}" && "$value" != '""' && "$value" != "''" ]] || fail "Required Compose variable $variable is empty."

  if [[ "$variable" == "SA_PASSWORD" && "$TEMPLATE_ONLY" == false ]]; then
    case "$value" in
      LOCAL_ONLY_CHANGE_BEFORE_USE_123!|ChangeThisStrongPassword!123)
        fail "deployment/.env still uses a documented placeholder for SA_PASSWORD. Set a private value."
        ;;
    esac
  fi
done <<< "$required_vars"

rendered_file="$(mktemp "${TMPDIR:-/tmp}/karting-compose-config.XXXXXX")"
trap 'rm -f "$rendered_file"' EXIT

# Render privately to validate interpolation; the resolved config can contain credentials.
docker compose --env-file "$ENV_SOURCE" --file "$COMPOSE_FILE" config > "$rendered_file"

if [[ "$TEMPLATE_ONLY" == true ]]; then
  echo "Compose configuration is valid with deployment/.env.example."
  echo "Template-only check: no real .env was validated; this does not establish runtime readiness."
else
  echo "Compose configuration is valid using deployment/.env."
  echo "No services were started; service readiness was not tested."
fi
