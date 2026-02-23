#!/usr/bin/env bash

set -euo pipefail

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
COMPOSE_FILE="$BASE_DIR/deployment/docker-compose.yml"
ENV_TEMPLATE="$BASE_DIR/deployment/.env.example"
ENV_FILE="$BASE_DIR/deployment/.env"
RENDERED_FILE="/tmp/karting-compose-config.out"

if ! command -v docker >/dev/null 2>&1; then
  echo "docker is required to validate deployment runtime."
  exit 1
fi

if [[ ! -f "$COMPOSE_FILE" ]]; then
  echo "Missing compose file: deployment/docker-compose.yml"
  exit 1
fi

if [[ ! -f "$ENV_TEMPLATE" ]]; then
  echo "Missing env template: deployment/.env.example"
  exit 1
fi

if [[ ! -d "$BASE_DIR/deployment/sql/schema" ]]; then
  echo "Missing deployment/sql/schema directory."
  exit 1
fi

if [[ ! -d "$BASE_DIR/deployment/sql/stored-procedures" ]]; then
  echo "Missing deployment/sql/stored-procedures directory."
  exit 1
fi

if [[ ! -f "$ENV_FILE" ]]; then
  echo "Missing deployment/.env. Copy deployment/.env.example to deployment/.env first."
  exit 1
fi

if grep -Eq '^SA_PASSWORD=ChangeThisStrongPassword!123$' "$ENV_FILE"; then
  echo "deployment/.env still uses default SA_PASSWORD. Set a strong value before startup."
  exit 1
fi

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" config > "$RENDERED_FILE"

echo "Compose configuration is valid."
echo "Rendered output: $RENDERED_FILE"
echo "Next step: deployment/scripts/apply-sql-scripts.sh"
