#!/usr/bin/env bash

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$BASE_DIR"

PLAN_DIR="$BASE_DIR/plan/stages"
TASKS_DIR="$BASE_DIR/tasks"
STAGE_LOG="$BASE_DIR/information/_stage-registry.log"

echo "🧠 Starting planner cycle..."

# ----------------------------------------
# 1) Validate git repo
# ----------------------------------------

if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
  echo "❌ Not inside a git repository."
  exit 1
fi

CURRENT_BRANCH=$(git branch --show-current)

if [[ "$CURRENT_BRANCH" == "development" || -z "$CURRENT_BRANCH" ]]; then
  echo "❌ Planner cannot run on development branch."
  exit 2
fi

echo "🌿 Current branch: $CURRENT_BRANCH"

# ----------------------------------------
# 2) Ensure coder not currently executing
# ----------------------------------------

ACTIVE_TASK=$(find "$TASKS_DIR/in-progress" -type f -name "*.md" | head -n 1 || true)

if [[ -n "$ACTIVE_TASK" ]]; then
  echo "⛔ Planner blocked: coder task in progress."
  echo "Active task: $(basename "$ACTIVE_TASK")"
  exit 3
fi

# ----------------------------------------
# 3) Detect ACTIVE mini plan
# ----------------------------------------

MINI_PLAN=$(grep -l "^Status: Active" "$PLAN_DIR"/mini-plan-stage-*.md 2>/dev/null | head -n 1 || true)

if [[ -z "$MINI_PLAN" ]]; then
  echo "❌ No ACTIVE mini plan found."
  exit 4
fi

STAGE_NAME=$(basename "$MINI_PLAN" .md)

echo "📄 Active stage detected: $STAGE_NAME"

# ----------------------------------------
# 4) Log planner cycle
# ----------------------------------------

TIMESTAMP=$(date "+%Y-%m-%d %H:%M")

mkdir -p "$BASE_DIR/information"

echo "$TIMESTAMP | $STAGE_NAME | $CURRENT_BRANCH | PLANNER-CYCLE | start-planner.sh" >> "$STAGE_LOG"

echo "📝 Planner cycle registered in information/_stage-registry.log"

# ----------------------------------------
# 5) Output execution context (for Continue)
# ----------------------------------------

echo ""
echo "==============================================="
echo "PLANNER EXECUTION CONTEXT"
echo "==============================================="
echo ""
echo "READ THESE FILES FIRST:"
echo " - rules/00-runtime-directives.md"
echo " - rules/10-code-architecture.md"
echo " - rules/20-repository-workflow.md"
echo " - rules/90-anti-patterns.md"
echo ""
echo "ACTIVE MINI PLAN:"
echo " - $MINI_PLAN"
echo ""
echo "OBJECTIVES:"
echo " - information/objectives.md"
echo ""
echo "INSTRUCTIONS:"
echo " - Generate planner tasks if needed"
echo " - Generate coder tasks in tasks/backlog/"
echo " - Move ONLY ONE coder task to tasks/ready/"
echo " - DO NOT create branches"
echo " - DO NOT modify src/ or test/"
echo ""
echo "==============================================="

echo "✔ Planner cycle ready."