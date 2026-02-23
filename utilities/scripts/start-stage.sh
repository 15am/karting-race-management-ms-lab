#!/usr/bin/env bash

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$BASE_DIR"

PLAN_DIR="$BASE_DIR/plan/stages"
TASKS_DIR="$BASE_DIR/tasks"
CONTEXT_DIR="$BASE_DIR/context"

echo "🚀 Starting feature stage..."

# ----------------------------------------
# 1) Validate git repo
# ----------------------------------------

if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
  echo "❌ Not inside a git repository."
  exit 1
fi

CURRENT_BRANCH=$(git branch --show-current)

if [[ "$CURRENT_BRANCH" != "development" ]]; then
  echo "❌ Must start stage from 'development'. Current branch: $CURRENT_BRANCH"
  exit 2
fi

# ----------------------------------------
# 2) Ensure no stage active
# ----------------------------------------

if [[ -f "$CONTEXT_DIR/_current-stage.md" ]]; then
  echo "❌ A stage is already active."
  exit 3
fi

# ----------------------------------------
# 3) Ensure no active tasks
# ----------------------------------------

ACTIVE_TASK=$(find "$TASKS_DIR/in-progress" -type f -name "*.md" | head -n 1 || true)

if [[ -n "$ACTIVE_TASK" ]]; then
  echo "❌ Cannot start new stage. Task in progress detected."
  exit 4
fi

# ----------------------------------------
# 4) Locate first Ready mini plan
# ----------------------------------------

MINI_PLAN=$(grep -l "^Status: Ready" "$PLAN_DIR"/mini-plan-stage-*.md 2>/dev/null | sort | head -n 1)

if [[ -z "$MINI_PLAN" ]]; then
  echo "❌ No mini plan with Status: Ready found."
  exit 5
fi

echo "📄 Using mini plan: $(basename "$MINI_PLAN")"

# ----------------------------------------
# 5) Extract metadata
# ----------------------------------------

BRANCH_NAME=$(grep "^BranchName:" "$MINI_PLAN" | cut -d ':' -f2- | xargs)
STAGE_NAME=$(grep "^StageName:" "$MINI_PLAN" | cut -d ':' -f2- | xargs)
STAGE_NUMBER=$(grep "^StageNumber:" "$MINI_PLAN" | cut -d ':' -f2- | xargs)

if [[ -z "$BRANCH_NAME" || -z "$STAGE_NAME" ]]; then
  echo "❌ BranchName or StageName missing in mini plan."
  exit 6
fi

echo "🌿 Target branch: $BRANCH_NAME"

# ----------------------------------------
# 6) Create or checkout branch
# ----------------------------------------

if git show-ref --verify --quiet "refs/heads/$BRANCH_NAME"; then
  echo "🔁 Branch exists. Checking out..."
  git checkout "$BRANCH_NAME"
else
  echo "🌱 Creating new branch from development..."
  git checkout -b "$BRANCH_NAME"
fi

# ----------------------------------------
# 7) Mark stage active
# ----------------------------------------

mkdir -p "$CONTEXT_DIR"

echo "$STAGE_NAME" > "$CONTEXT_DIR/_current-stage.md"

sed -i "s/^Status:.*/Status: Active/" "$MINI_PLAN"

# ----------------------------------------
# 8) Log registry (append-only)
# ----------------------------------------

STAGE_LOG="$BASE_DIR/information/_stage-registry.log"
TIMESTAMP=$(date "+%Y-%m-%d %H:%M")

echo "$TIMESTAMP | $STAGE_NAME | $BRANCH_NAME | STARTED | start-stage.sh" >> "$STAGE_LOG"

# ----------------------------------------
# 9) Done
# ----------------------------------------

echo "✔ Stage started → $STAGE_NAME"
echo "✔ Branch ready → $BRANCH_NAME"