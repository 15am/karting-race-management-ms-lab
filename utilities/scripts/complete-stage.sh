#!/usr/bin/env bash

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$BASE_DIR"

echo "🏁 Completing stage..."

# ----------------------------------------
# 1) Ensure git repo
# ----------------------------------------

if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
  echo "❌ Not inside a git repository."
  exit 1
fi

CURRENT_BRANCH=$(git branch --show-current)

if [[ "$CURRENT_BRANCH" == "development" || "$CURRENT_BRANCH" == "main" ]]; then
  echo "❌ Cannot complete stage from branch: $CURRENT_BRANCH"
  exit 2
fi

FEATURE_BRANCH="$CURRENT_BRANCH"

echo "🌿 Current feature branch: $FEATURE_BRANCH"

# ----------------------------------------
# 2) Run stage validation
# ----------------------------------------

echo "🔍 Running stage validation..."
"$BASE_DIR/utilities/scripts/validate-stage.sh"
VALIDATION_EXIT=$?

if [[ "$VALIDATION_EXIT" -ne 0 ]]; then
  echo "❌ Stage validation failed. Abort."
  exit 3
fi

echo "✔ Stage validation passed"

# ----------------------------------------
# 3) Detect mini plan
# ----------------------------------------

PLAN_DIR="$BASE_DIR/plan/stages"

MINI_PLAN=$(find "$PLAN_DIR" -maxdepth 1 -type f -name "mini-plan-stage-*.md" | sort | head -n 1)

if [[ -z "$MINI_PLAN" ]]; then
  echo "❌ Mini plan not found."
  exit 4
fi

STAGE_NAME=$(basename "$MINI_PLAN" .md)

echo "📄 Stage detected: $STAGE_NAME"

# ----------------------------------------
# 4) Merge feature -> development
# ----------------------------------------

echo "🔀 Switching to development..."
git checkout development

echo "🔗 Merging $FEATURE_BRANCH into development..."
git merge "$FEATURE_BRANCH" --no-ff -m "stage($STAGE_NAME): complete"

echo "✔ Merge complete"

# ----------------------------------------
# 5) Record last completed stage (replaces mark-stage-complete.sh)
# ----------------------------------------

mkdir -p "$BASE_DIR/context"

echo "$STAGE_NAME" > "$BASE_DIR/context/_last-completed-stage.md"

echo "📝 Stage recorded in context/_last-completed-stage.md"

# ----------------------------------------
# 6) Done
# ----------------------------------------

echo "🎉 Stage completed successfully"
echo "📌 Repo is now on development branch"

STAGE_LOG="$BASE_DIR/information/_stage-registry.log"
TIMESTAMP=$(date "+%Y-%m-%d %H:%M")

echo "$TIMESTAMP | $STAGE_NAME | $FEATURE_BRANCH | MERGED | complete-stage.sh" >> "$STAGE_LOG"