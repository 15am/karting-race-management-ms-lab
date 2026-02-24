#!/usr/bin/env bash

set -euo pipefail

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
COMPOSE_FILE="$BASE_DIR/deployment/docker-compose.yml"
ENV_FILE="$BASE_DIR/deployment/.env"
SQL_ROOT="$BASE_DIR/deployment/sql"
SCHEMA_DIR="$SQL_ROOT/schema"
PROCEDURE_DIR="$SQL_ROOT/stored-procedures"
SEED_DIR="$SQL_ROOT/seed"
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
TIMEOUT_SECONDS=120

if ! command -v docker >/dev/null 2>&1; then
  echo "docker is required."
  exit 1
fi

if [[ ! -f "$COMPOSE_FILE" ]]; then
  echo "Missing compose file: deployment/docker-compose.yml"
  exit 1
fi

if [[ ! -f "$ENV_FILE" ]]; then
  echo "Missing deployment/.env. Copy deployment/.env.example first."
  exit 1
fi

if grep -Eq '^SA_PASSWORD=ChangeThisStrongPassword!123$' "$ENV_FILE"; then
  echo "deployment/.env still uses default SA_PASSWORD. Set a strong value before startup."
  exit 1
fi

if [[ ! -d "$SCHEMA_DIR" ]]; then
  echo "Missing SQL schema directory: deployment/sql/schema"
  exit 1
fi

if [[ ! -d "$PROCEDURE_DIR" ]]; then
  echo "Missing SQL stored-procedure directory: deployment/sql/stored-procedures"
  exit 1
fi

if [[ ! -d "$SEED_DIR" ]]; then
  echo "Missing SQL seed directory: deployment/sql/seed"
  exit 1
fi

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d sqlserver

echo "Waiting for SQL Server readiness..."
started_at=$(date +%s)

while true; do
  if docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T sqlserver /bin/bash -lc "$SQLCMD -S localhost -U sa -P \"\$MSSQL_SA_PASSWORD\" -Q \"SELECT 1\" -C >/dev/null"; then
    break
  fi

  now=$(date +%s)
  elapsed=$((now - started_at))
  if (( elapsed >= TIMEOUT_SECONDS )); then
    echo "Timed out waiting for SQL Server after ${TIMEOUT_SECONDS}s."
    exit 1
  fi

  sleep 2
done

run_script() {
  local host_path="$1"
  local relative_path="${host_path#"$SQL_ROOT"/}"
  local container_path="/opt/karting/sql/$relative_path"

  echo "Applying $relative_path"
  docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T sqlserver /bin/bash -lc "$SQLCMD -S localhost -U sa -P \"\$MSSQL_SA_PASSWORD\" -C -i \"$container_path\""
}

shopt -s nullglob
schema_scripts=("$SCHEMA_DIR"/*.sql)
procedure_scripts=("$PROCEDURE_DIR"/*.sql)
seed_scripts=("$SEED_DIR"/*.sql)

if (( ${#schema_scripts[@]} == 0 )); then
  echo "No schema scripts found in deployment/sql/schema."
  exit 1
fi

if (( ${#procedure_scripts[@]} == 0 )); then
  echo "No stored procedure scripts found in deployment/sql/stored-procedures."
  exit 1
fi

if (( ${#seed_scripts[@]} == 0 )); then
  echo "No seed scripts found in deployment/sql/seed."
  exit 1
fi

for script in "${schema_scripts[@]}"; do
  run_script "$script"
done

for script in "${procedure_scripts[@]}"; do
  run_script "$script"
done

for script in "${seed_scripts[@]}"; do
  run_script "$script"
done

echo "SQL scripts applied successfully."
