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

echo "### TARGET FILES CONTEXT"
echo "Task: $TASK_ID"
echo ""

# ----------------------------------------
# Extract AllowedWriteTargets
# ----------------------------------------

TARGETS=$(awk '
/AllowedWriteTargets:/ {flag=1; next}
/^$/ && flag {flag=0}
flag && /^- / {print $2}
' "$TASK_FILE")

if [[ -z "$TARGETS" ]]; then
  echo "No AllowedWriteTargets found."
  exit 0
fi

# ----------------------------------------
# Read files from allowed targets
# ----------------------------------------

for target in $TARGETS; do

  CLEAN_TARGET=${target%\*\*}

  if [[ -d "$BASE_DIR/$CLEAN_TARGET" ]]; then

    find "$BASE_DIR/$CLEAN_TARGET" -type f \( -name "*.cs" -o -name "*.csproj" -o -name "*.sln" \) | while read -r file; do

      echo "FILE: ${file#$BASE_DIR/}"
      echo "--------------------------------"

      cat "$file"

      echo ""
      echo ""

    done

  fi

done