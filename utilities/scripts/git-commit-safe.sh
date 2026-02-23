#!/usr/bin/env bash
#
# Usage: ./git-commit-safe.sh

set -e

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SCRIPTS_DIR="$BASE_DIR/utilities/scripts"

echo "🛡️ Starting Safe Commit process..."

# 1. Branch Protection
CURRENT_BRANCH=$(git branch --show-current)
if [[ "$CURRENT_BRANCH" == "development" || "$CURRENT_BRANCH" == "main" ]]; then
  echo "❌ Error: Direct commits to '$CURRENT_BRANCH' are forbidden."
  exit 1
fi

# 2. Pre-commit Validation
echo "🔍 Running task validation..."
if ! "$SCRIPTS_DIR/validate-task.sh"; then
  echo "❌ Error: Task validation failed."
  exit 2
fi

# 3. Metadata Extraction
TASK_ID=$("$SCRIPTS_DIR/current-task.sh" coder 2>/dev/null || true)

if [[ -z "$TASK_ID" ]]; then
    TYPE="chore"
    SUBJECT="miscellaneous updates"
    SCOPE="repo"
else
    TASK_FILE=$(find "$BASE_DIR/tasks" -type f -name "$TASK_ID.md" | head -n 1)
    
    # Extract TaskType
    RAW_TYPE=$(grep "^TaskType:" "$TASK_FILE" | cut -d':' -f2 | xargs | tr '[:upper:]' '[:lower:]')
    case "$RAW_TYPE" in
        "feature") TYPE="feat" ;;
        "fix")     TYPE="fix" ;;
        "refactor") TYPE="refactor" ;;
        *)          TYPE="feat" ;;
    esac

    SUBJECT=$(grep -m 1 "^## 2. Objective" -A 1 "$TASK_FILE" | tail -n 1 | xargs)
    SCOPE="$TASK_ID"
fi

# 4. Automatic Body Generation (What changed?)
# We generate a summary of staged changes (--cached)
FILE_SUMMARY=$(git diff --cached --stat --compact-summary)
DETAILED_CHANGES=$(git diff --cached --name-status)

# Format the final message
# Note: Conventional commits require a blank line between header and body
HEADER="$TYPE($SCOPE): $SUBJECT"
BODY="Changes implemented:
$DETAILED_CHANGES

Summary:
$FILE_SUMMARY"

FULL_MESSAGE=$(printf "%s\n\n%s" "$HEADER" "$BODY")

# 5. Execution
echo "📝 Generated Commit Message:"
echo "--------------------------------"
echo "$FULL_MESSAGE"
echo "--------------------------------"

git add .

if git diff --cached --quiet; then
    echo "ℹ️ Nothing to commit, working tree clean."
else
    # Use -m with the full variable to include newlines
    git commit -m "$FULL_MESSAGE"
    echo "✅ Successfully committed to $CURRENT_BRANCH"
fi