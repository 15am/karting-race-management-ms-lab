#!/usr/bin/env bash

set -e

ROLE="$1" # planner | coder

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
TASKS_DIR="$BASE_DIR/tasks"
MOVE_SCRIPT="$BASE_DIR/utilities/scripts/move-task.sh"

if [[ -z "$ROLE" ]]; then
  echo "Usage: current-task.sh [planner|coder]"
  exit 1
fi

# ----------------------------------------
# Helpers
# ----------------------------------------

task_matches_role() {
  FILE="$1"

  if [[ "$ROLE" == "planner" ]] && grep -q "ID: Planner-" "$FILE"; then
    return 0
  fi

  if [[ "$ROLE" == "coder" ]] && grep -q "ID: Coder-" "$FILE"; then
    return 0
  fi

  return 1
}

dependencies_satisfied() {
  FILE="$1"

  DEP=$(grep "^DependsOn:" "$FILE" | sed 's/DependsOn:[[:space:]]*//')

  if [[ -z "$DEP" || "$DEP" == "None" ]]; then
    return 0
  fi

  DEP_FILE="$TASKS_DIR/done/${DEP}.md"

  if [[ -f "$DEP_FILE" ]]; then
    return 0
  fi

  return 1
}

# ----------------------------------------
# 1) Check in-progress first
# ----------------------------------------

for FILE in "$TASKS_DIR/in-progress"/*.md; do
  [[ -e "$FILE" ]] || continue

  if task_matches_role "$FILE"; then
    basename "$FILE" .md
    exit 0
  fi
done

# ----------------------------------------
# 2) Look into ready
# ----------------------------------------

for FILE in $(ls "$TASKS_DIR/ready"/*.md 2>/dev/null | sort); do

  if ! task_matches_role "$FILE"; then
    continue
  fi

  if ! dependencies_satisfied "$FILE"; then
    continue
  fi

  TASK_ID=$(basename "$FILE" .md)

  # Auto move ready -> in-progress
  "$MOVE_SCRIPT" "$TASK_ID" in-progress auto

  echo "$TASK_ID"
  exit 0

done

# ----------------------------------------
# 3) Nothing found
# ----------------------------------------

echo "No active task for role: $ROLE"
exit 1