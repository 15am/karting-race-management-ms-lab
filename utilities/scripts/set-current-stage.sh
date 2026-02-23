#!/usr/bin/env bash

set -e

STAGE_NAME="$1"
BRANCH_NAME="$2"
MINI_PLAN_PATH="$3"

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
CONTEXT_DIR="$BASE_DIR/context"
CURRENT_STAGE_FILE="$CONTEXT_DIR/_current-stage.md"

if [[ -z "$STAGE_NAME" || -z "$BRANCH_NAME" || -z "$MINI_PLAN_PATH" ]]; then
  echo "Usage: set-current-stage.sh <stage> <branch> <mini-plan-path>"
  exit 1
fi

mkdir -p "$CONTEXT_DIR"

cat <<EOF > "$CURRENT_STAGE_FILE"
Stage: $STAGE_NAME
Branch: $BRANCH_NAME
MiniPlan: $MINI_PLAN_PATH
Status: Active
EOF

echo "✔ Current stage updated → context/_current-stage.md"