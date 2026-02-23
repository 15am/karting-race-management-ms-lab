#!/usr/bin/env bash

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$BASE_DIR"

STAGE="$1"

if [[ -z "$STAGE" ]]; then
  echo "Usage: is-stage-complete.sh Stage-X"
  exit 1
fi

echo "🔎 Checking stage completion: $STAGE"

PENDING=$(grep -R "Stage: $STAGE" tasks/backlog tasks/ready tasks/in-progress tasks/review 2>/dev/null || true)

if [[ -z "$PENDING" ]]; then
  echo "✔ Stage complete"
  exit 0
else
  echo "⏳ Stage still has pending tasks"
  exit 2
fi