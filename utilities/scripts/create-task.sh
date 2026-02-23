#!/usr/bin/env bash

set -e

ROLE="$1"
OBJECTIVE="$2"
STAGE="$3"

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

INDEX_FILE="$BASE_DIR/information/task-index.log"
BACKLOG_DIR="$BASE_DIR/tasks/backlog"
TEMPLATE_DIR="$BASE_DIR/utilities/templates"

PLANNER_TEMPLATE="$TEMPLATE_DIR/task-template-planner.md"
CODER_TEMPLATE="$TEMPLATE_DIR/task-template-coder.md"

# -----------------------------
# 1) Validate arguments
# -----------------------------

if [[ -z "$ROLE" || -z "$OBJECTIVE" || -z "$STAGE" ]]; then
  echo "Usage: create-task.sh <planner|coder> \"objective\" <Stage-X>"
  exit 1
fi

if [[ "$ROLE" != "planner" && "$ROLE" != "coder" ]]; then
  echo "Role must be 'planner' or 'coder'"
  exit 1
fi

mkdir -p "$BACKLOG_DIR"
mkdir -p "$(dirname "$INDEX_FILE")"

# -----------------------------
# 2) Get next task number
# -----------------------------

if [[ ! -f "$INDEX_FILE" ]]; then
  echo "0000|INIT|Stage-0" > "$INDEX_FILE"
fi

LAST_LINE=$(tail -n 1 "$INDEX_FILE")
LAST_ID=$(echo "$LAST_LINE" | cut -d'|' -f1)

NEXT_NUM=$(printf "%04d" $((10#$LAST_ID + 1)))

# -----------------------------
# 3) Build Task ID + Template
# -----------------------------

if [[ "$ROLE" == "planner" ]]; then
  TASK_ID="Planner-TASK-$NEXT_NUM"
  TEMPLATE="$PLANNER_TEMPLATE"
else
  TASK_ID="Coder-TASK-$NEXT_NUM"
  TEMPLATE="$CODER_TEMPLATE"
fi

TARGET_FILE="$BACKLOG_DIR/$TASK_ID.md"

# -----------------------------
# 4) Create task from template
# -----------------------------

cp "$TEMPLATE" "$TARGET_FILE"

# Replace Metadata fields safely
sed -i "s/ID:.*/ID: $TASK_ID/" "$TARGET_FILE"
sed -i "s/Stage:.*/Stage: $STAGE/" "$TARGET_FILE"
sed -i "0,/Short, concrete objective./s//${OBJECTIVE}/" "$TARGET_FILE"

# -----------------------------
# 5) Append to task index
# -----------------------------

echo "$NEXT_NUM|$ROLE|$STAGE" >> "$INDEX_FILE"

echo "✔ Created task: $TASK_ID"
echo "→ $TARGET_FILE"