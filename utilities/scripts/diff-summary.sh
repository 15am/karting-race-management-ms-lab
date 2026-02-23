#!/usr/bin/env bash

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$BASE_DIR"

CURRENT_TASK=$("$BASE_DIR/utilities/scripts/current-task.sh" coder 2>/dev/null || true)

BRANCH=$(git rev-parse --abbrev-ref HEAD)
BASE_BRANCH="development"

echo "========================================"
echo "DIFF SUMMARY"
echo "========================================"
echo "Task: ${CURRENT_TASK:-N/A}"
echo "Branch: $BRANCH"
echo "Base: $BASE_BRANCH"
echo ""

# ----------------------------------------
# Validate repo state
# ----------------------------------------

if ! git rev-parse "$BASE_BRANCH" >/dev/null 2>&1; then
  echo "ERROR: Base branch '$BASE_BRANCH' not found."
  exit 1
fi

# ----------------------------------------
# Commit Summary
# ----------------------------------------

echo "---- Commits since $BASE_BRANCH ----"
git log --oneline "$BASE_BRANCH..HEAD" || true
echo ""

# ----------------------------------------
# File Change Summary
# ----------------------------------------

echo "---- Files Changed ----"
git diff --name-status "$BASE_BRANCH..HEAD" || true
echo ""

# ----------------------------------------
# Line Stats
# ----------------------------------------

echo "---- Line Stats ----"
git diff --stat "$BASE_BRANCH..HEAD" || true
echo ""

# ----------------------------------------
# AllowedWriteTargets Quick Check
# (soft validation — validate-stage hará la fuerte)
# ----------------------------------------

echo "---- Write Scope Check ----"

git diff --name-only "$BASE_BRANCH..HEAD" | while read -r FILE; do

  if [[ "$FILE" == src/* ]] || \
     [[ "$FILE" == test/* ]] || \
     [[ "$FILE" == deployment/* ]] || \
     [[ "$FILE" == docs/* ]]; then
      echo "[OK] $FILE"
  else
      echo "[WARN] Outside allowed scope -> $FILE"
  fi

done

echo ""
echo "========================================"