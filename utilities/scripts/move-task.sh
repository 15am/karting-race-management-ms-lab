#!/usr/bin/env bash

set -e

TASK_ID="$1"
TARGET="$2"
COMMENT="$3"

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
TASKS_DIR="$BASE_DIR/tasks"

VALID_COLUMNS=("backlog" "ready" "in-progress" "review" "done")

# -----------------------------
# 1) Validate input
# -----------------------------

if [[ -z "$TASK_ID" || -z "$TARGET" ]]; then
  echo "Usage: move-task.sh <TASK_ID> <target-column> [comment]"
  exit 1
fi

if [[ ! " ${VALID_COLUMNS[*]} " =~ " ${TARGET} " ]]; then
  echo "Invalid target column: $TARGET"
  exit 1
fi

# -----------------------------
# 2) Locate task file
# -----------------------------

CURRENT_FILE=$(find "$TASKS_DIR" -type f -name "$TASK_ID.md" | head -n 1)

if [[ -z "$CURRENT_FILE" ]]; then
  echo "Task not found: $TASK_ID"
  exit 1
fi

CURRENT_DIR=$(basename "$(dirname "$CURRENT_FILE")")
TARGET_FILE="$TASKS_DIR/$TARGET/$TASK_ID.md"

if [[ "$CURRENT_DIR" == "$TARGET" ]]; then
  echo "Task already in $TARGET"
  exit 0
fi

# -----------------------------
# 3) Update Status inside file
# -----------------------------

sed -i "s/^Status:.*/Status: $TARGET/" "$CURRENT_FILE"

# -----------------------------
# 4) Append Execution History
# -----------------------------

TIMESTAMP=$(date "+%Y-%m-%d %H:%M")

if [[ -z "$COMMENT" ]]; then
  HISTORY_LINE="- [$TIMESTAMP] MOVED $CURRENT_DIR → $TARGET"
else
  HISTORY_LINE="- [$TIMESTAMP] MOVED $CURRENT_DIR → $TARGET | $COMMENT"
fi

echo "" >> "$CURRENT_FILE"
echo "$HISTORY_LINE" >> "$CURRENT_FILE"

# -----------------------------
# 5) Move file
# -----------------------------

mv "$CURRENT_FILE" "$TARGET_FILE"

echo "✔ $TASK_ID moved: $CURRENT_DIR → $TARGET"