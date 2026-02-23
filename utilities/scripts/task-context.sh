#!/usr/bin/env bash

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

CURRENT_TASK_SCRIPT="$BASE_DIR/utilities/scripts/current-task.sh"
TASK_ID=$("$CURRENT_TASK_SCRIPT")

TASK_FILE=$(find "$BASE_DIR/tasks" -type f -name "$TASK_ID.md" | head -n 1)

if [[ -z "$TASK_FILE" ]]; then
  echo "Task file not found."
  exit 1
fi

echo "### TASK CONTEXT"
echo "Task: $TASK_ID"
echo ""

# ----------------------------------------
# Extract relevant sections only
# ----------------------------------------

awk '
/^## 1\. Metadata/ {flag=1}
flag && /^## 8\. Execution History/ {flag=0}
flag {print}
' "$TASK_FILE"