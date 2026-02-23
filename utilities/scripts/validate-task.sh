#!/usr/bin/env bash

set -e

extract_allowed_targets() {
  local task_file="$1"

  awk '
    /AllowedWriteTargets:/ {flag=1; next}
    flag && NF==0 {flag=0}
    flag && /^\-/ {
      gsub("- ","")
      gsub("\\*\\*","")
      print "^"$0
    }
  ' "$task_file"
}

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$BASE_DIR"

echo "🔎 Running task validation..."

# --------------------------------------------------
# 0) Detect current branch
# --------------------------------------------------

CURRENT_BRANCH=$(git branch --show-current)

if [[ "$CURRENT_BRANCH" == "development" || "$CURRENT_BRANCH" == "main" ]]; then
  echo "❌ Cannot validate task on protected branch."
  exit 5
fi

# --------------------------------------------------
# 1) Detect current task
# --------------------------------------------------

CURRENT_TASK=$("$BASE_DIR/utilities/scripts/current-task.sh" coder 2>/dev/null || true)

if [[ -z "$CURRENT_TASK" ]]; then
  echo "❌ No active coder task found."
  exit 6
fi

echo "🧩 Current task: $CURRENT_TASK"

# --------------------------------------------------
# 2) Get modified files
# --------------------------------------------------

MODIFIED_FILES=$(git diff --name-only development..HEAD || true)

if [[ -z "$MODIFIED_FILES" ]]; then
  echo "⚠ No changes detected."
fi

# --------------------------------------------------
# 3) Allowed targets validation
# --------------------------------------------------

TASK_FILE=$(find tasks -type f -name "$CURRENT_TASK.md" | head -n1)

ALLOWED_REGEX=$(extract_allowed_targets "$TASK_FILE" | paste -sd "|" -)

FORBIDDEN_FOUND=0

while read -r file; do
  [[ -z "$file" ]] && continue

  if [[ ! "$file" =~ $ALLOWED_REGEX ]]; then
    echo "❌ Forbidden file modified: $file"
    FORBIDDEN_FOUND=1
  else
    echo "✔ Allowed file: $file"
  fi
done <<< "$MODIFIED_FILES"

# --------------------------------------------------
# 4) Generate diff summary (Planner context)
# --------------------------------------------------

echo "📊 Generating diff summary..."

"$BASE_DIR/utilities/scripts/diff-summary.sh" > "$BASE_DIR/context/_task-diff.md" 2>&1 || true

# --------------------------------------------------
# 5) Basic rule checks (Object Calisthenics Guardrails)
# --------------------------------------------------
ANTI_PATTERN_HIT=0

# Only validate modified C# files
CS_MODIFIED=$(echo "$MODIFIED_FILES" | grep "\.cs$" || true)

for file in $CS_MODIFIED; do
    # RULE: Else Ban
    # Scans for 'else' keyword while ignoring simple comments
    if grep -vE "^\s*//" "$file" | grep -qE "\belse\b"; then
        echo "⚠ Anti-pattern detected in $file: 'else' statement forbidden. Use guard clauses."
        ANTI_PATTERN_HIT=1
    fi

    # RULE: Max 2 fields
    # Counts private field declarations (heuristic: private ... ;)
    FIELD_COUNT=$(grep -vE "^\s*//" "$file" | grep -E "^\s*private\s+[^(\n]*;" | wc -l || echo 0)
    if [ "$FIELD_COUNT" -gt 2 ]; then
        echo "⚠ Anti-pattern detected in $file: $FIELD_COUNT fields detected (Max: 2). Extract Value Objects."
        ANTI_PATTERN_HIT=1
    fi

    # RULE: Max 40 lines
    # Counts physical lines in the file to enforce small, cohesive classes
    LINE_COUNT=$(wc -l < "$file")
    if [ "$LINE_COUNT" -gt 120 ]; then
        echo "⚠ Anti-pattern detected in $file: $LINE_COUNT lines detected (Max: 120). Split the class."
        ANTI_PATTERN_HIT=1
    fi
done

# Original check: No Console.WriteLine in API
if grep -R "Console.WriteLine" api/ >/dev/null 2>&1; then
  echo "⚠ Anti-pattern detected: Console.WriteLine usage found in api/"
  ANTI_PATTERN_HIT=1
fi

# --------------------------------------------------
# 6) Exit codes
# --------------------------------------------------

EXIT_CODE=0

if [[ "$FORBIDDEN_FOUND" -eq 1 ]]; then
  EXIT_CODE=1
elif [[ "$ANTI_PATTERN_HIT" -eq 1 ]]; then
  EXIT_CODE=2
fi

# --------------------------------------------------
# 7) Output summary
# --------------------------------------------------

echo ""
echo "🧾 Task Validation Summary"
echo "Branch: $CURRENT_BRANCH"
echo "Task: $CURRENT_TASK"
echo "ExitCode: $EXIT_CODE"

exit "$EXIT_CODE"