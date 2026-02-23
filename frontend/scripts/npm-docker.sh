#!/usr/bin/env bash

set -euo pipefail

FRONTEND_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
NODE_IMAGE="${NODE_IMAGE:-node:20}"
FRONTEND_PORT="${FRONTEND_PORT:-5173}"

if [[ $# -eq 0 ]]; then
  echo "Usage: scripts/npm-docker.sh <npm-args...>"
  echo "Example: scripts/npm-docker.sh install"
  exit 1
fi

DOCKER_ARGS=(
  run
  --rm
  --security-opt
  apparmor=unconfined
  -e
  NPM_CONFIG_CACHE=/tmp/.npm
  -e
  HOME=/tmp
  -u
  "$(id -u):$(id -g)"
  -v
  "${FRONTEND_DIR}:/workspace"
  -w
  /workspace
)

if [[ "$1" == "run" && "${2:-}" == "dev" ]]; then
  docker "${DOCKER_ARGS[@]}" -p "${FRONTEND_PORT}:${FRONTEND_PORT}" "${NODE_IMAGE}" \
    npm run dev -- --host 0.0.0.0 --port "${FRONTEND_PORT}"
  exit 0
fi

docker "${DOCKER_ARGS[@]}" "${NODE_IMAGE}" npm "$@"
