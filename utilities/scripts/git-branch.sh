#!/usr/bin/env bash
#
# Usage: ./git-branch.sh from-mini-plan [base_branch]
# Usage: ./git-branch.sh status

set -e

# 1. Configuration (Parametrizable defaults)
DEFAULT_BASE="development"
PLAN_DIR="./plan/stages" # Updated to match your folder structure

ACTION="$1"
BASE_BRANCH="${2:-$DEFAULT_BASE}"

get_branch_from_miniplan() {
  # Find the most recent mini-plan (Active or Ready)
  # Sorts by filename to get the latest stage
  MINI_PLAN=$(ls -v "$PLAN_DIR"/mini-plan-stage-*.md 2>/dev/null | tail -n 1)

  if [[ -z "$MINI_PLAN" ]]; then
    echo "❌ Error: No mini-plan found in $PLAN_DIR" >&2
    exit 2
  fi

  # Extract BranchName using a more robust grep/sed pattern
  BRANCH=$(grep "^BranchName:" "$MINI_PLAN" | cut -d':' -f2 | xargs)

  if [[ -z "$BRANCH" ]]; then
    echo "❌ Error: BranchName not defined in $MINI_PLAN" >&2
    exit 3
  fi

  echo "$BRANCH"
}

if [[ "$ACTION" == "from-mini-plan" ]]; then
  TARGET_BRANCH=$(get_branch_from_miniplan)

  echo "🌿 Preparing branch: $TARGET_BRANCH"

  # Check if we are in a git repo
  if ! git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
    echo "❌ Error: Not a git repository." >&2
    exit 1
  fi

  # Resilient Fetch/Pull (Only if remote exists)
  if git remote | grep -q "origin"; then
    echo "📡 Remote 'origin' detected. Syncing..."
    git fetch origin "$BASE_BRANCH" || true
    git checkout "$BASE_BRANCH"
    git pull origin "$BASE_BRANCH" || true
  else
    echo "ℹ️ No remote 'origin' found. Proceeding with local $BASE_BRANCH."
    git checkout "$BASE_BRANCH" || git checkout -b "$BASE_BRANCH"
  fi

  # Branch Creation Logic
  if git show-ref --quiet "refs/heads/$TARGET_BRANCH"; then
    echo "🔄 Branch '$TARGET_BRANCH' already exists. Switching..."
    git checkout "$TARGET_BRANCH"
  else
    echo "🌱 Creating new branch '$TARGET_BRANCH' from '$BASE_BRANCH'..."
    git checkout -b "$TARGET_BRANCH"
  fi

  echo "✅ Ready: $(git branch --show-current)"
  exit 0
fi

if [[ "$ACTION" == "status" ]]; then
  git branch --show-current
  exit 0
fi

echo "Usage:"
echo "  git-branch.sh from-mini-plan [base_branch]"
echo "  git-branch.sh status"
exit 1