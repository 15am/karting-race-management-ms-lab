#!/usr/bin/env bash
#
# Usage: ./emergency-stop.sh "Reason for stopping"

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SCRIPTS_DIR="$BASE_DIR/utilities/scripts"
TASKS_DIR="$BASE_DIR/tasks"

REASON="${1:-No reason provided}"

echo "🚨 EMERGENCY STOP TRIGGERED"
echo "Reason: $REASON"

# 1. Identify active task
CURRENT_TASK=$("$SCRIPTS_DIR/current-task.sh" coder 2>/dev/null || true)

# 2. Revert uncommitted changes
echo "🧹 Reverting local changes (git reset & clean)..."
git reset --hard HEAD
git clean -fd

# 3. Handle Task State
if [[ -n "$CURRENT_TASK" ]]; then
    echo "📋 Moving task $CURRENT_TASK back to backlog for analysis..."
    "$SCRIPTS_DIR/move-task.sh" "$CURRENT_TASK" "backlog" "EMERGENCY_STOP: $REASON"
fi

# 4. Registry Log
TIMESTAMP=$(date "+%Y-%m-%d %H:%M")
echo "$TIMESTAMP | $CURRENT_TASK | $(git branch --show-current) | EMERGENCY_STOP | Reason: $REASON" >> "$BASE_DIR/information/_stage-registry.log"

echo "✅ Environment cleaned. Ready for Planner re-assessment."