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

REPORT_FILE="$BASE_DIR/context/_stage-validation.md"

echo "🔍 Running stage validation..."

CURRENT_BRANCH=$(git branch --show-current)
CURRENT_TASK=$("$BASE_DIR/utilities/scripts/current-task.sh" coder 2>/dev/null || true)

# ----------------------------------------
# 0) Branch protection
# ----------------------------------------

if [[ "$CURRENT_BRANCH" == "development" || "$CURRENT_BRANCH" == "main" ]]; then
  echo "❌ Validation blocked: protected branch."
  exit 5
fi

# ----------------------------------------
# 1) Diff Summary
# ----------------------------------------

echo "📊 Running diff summary..."
"$BASE_DIR/utilities/scripts/diff-summary.sh" > /tmp/diff-summary.log 2>&1 || true

# ----------------------------------------
# 2) Detect modified files
# ----------------------------------------

# Locate the current task file to extract AllowedWriteTargets
TASK_FILE=$(find tasks -type f -name "$CURRENT_TASK.md" | head -n1)
ALLOWED_REGEX=$(extract_allowed_targets "$TASK_FILE" | paste -sd "|" -)
ALLOWED_OK=0

MODIFIED_FILES=$(git diff --name-only development..HEAD || true)

if [[ -n "$MODIFIED_FILES" ]]; then
  while read -r file; do
    [[ -z "$file" ]] && continue
    if [[ ! "$file" =~ $ALLOWED_REGEX ]]; then
      echo "❌ Forbidden modification detected: $file"
      ALLOWED_OK=1
    fi
  done <<< "$MODIFIED_FILES"
fi

# ----------------------------------------
# 3) dotnet build
# ----------------------------------------

BUILD_OK=0

echo "🏗 Running dotnet build..."

if ! dotnet build --nologo > /tmp/build.log 2>&1; then
  BUILD_OK=1
  echo "❌ Build failed"
else
  echo "✔ Build OK"
fi

# ----------------------------------------
# 4) dotnet test
# ----------------------------------------

TEST_OK=0

echo "🧪 Running dotnet test..."

if ! dotnet test --nologo > /tmp/test.log 2>&1; then
  TEST_OK=1
  echo "❌ Tests failed"
else
  echo "✔ Tests OK"
fi

# ----------------------------------------
# 5) Object Calisthenics Compliance (Global check)
# ----------------------------------------
CALISTHENICS_OK=0
echo "📐 Checking Object Calisthenics compliance..."

CS_MODIFIED=$(echo "$MODIFIED_FILES" | grep "\.cs$" || true)
for file in $CS_MODIFIED; do
    # Check Else Ban
    if grep -vE "^\s*//" "$file" | grep -qE "\belse\b"; then
        echo "⚠ Anti-pattern: 'else' found in $file"
        CALISTHENICS_OK=1
    fi
    # Check Line Count (Max 120) 
    if [ "$(wc -l < "$file")" -gt 120 ]; then
        echo "⚠ Anti-pattern: Too many lines in $file"
        CALISTHENICS_OK=1
    fi
done

# ----------------------------------------
# 6) Determine exit code
# ----------------------------------------
# 0: Success
# 1: Scope Violation
# 2: Build Failure
# 3: Test Failure
# 4: Multiple Failures
# 7: Calisthenics Violation 

EXIT_CODE=0
FAILURES_COUNT=$((ALLOWED_OK + BUILD_OK + TEST_OK + CALISTHENICS_OK))

if [[ "$FAILURES_COUNT" -gt 1 ]]; then
  EXIT_CODE=4
elif [[ "$ALLOWED_OK" -eq 1 ]]; then
  EXIT_CODE=1
elif [[ "$BUILD_OK" -eq 1 ]]; then
  EXIT_CODE=2
elif [[ "$TEST_OK" -eq 1 ]]; then
  EXIT_CODE=3
elif [[ "$CALISTHENICS_OK" -eq 1 ]]; then
  EXIT_CODE=7
fi

# ----------------------------------------
# 7) Prepare report
# ----------------------------------------
TIMESTAMP=$(date "+%Y-%m-%d %H:%M")

ALLOWED_STATUS=$([[ "$ALLOWED_OK" -eq 0 ]] && echo "OK" || echo "FAILED")
BUILD_STATUS=$([[ "$BUILD_OK" -eq 0 ]] && echo "OK" || echo "FAILED")
TEST_STATUS=$([[ "$TEST_OK" -eq 0 ]] && echo "OK" || echo "FAILED")
CALI_STATUS=$([[ "$CALISTHENICS_OK" -eq 0 ]] && echo "OK" || echo "FAILED")

REPORT_BLOCK=$(cat <<EOF

### $TIMESTAMP
Branch: $CURRENT_BRANCH
Task: $CURRENT_TASK
AllowedTargets: $ALLOWED_STATUS
Build: $BUILD_STATUS
Tests: $TEST_STATUS
Calisthenics: $CALI_STATUS
ExitCode: $EXIT_CODE
EOF
)

mkdir -p "$BASE_DIR/context"

if [[ ! -f "$REPORT_FILE" ]]; then
  echo "# Stage Validation Report" > "$REPORT_FILE"
  echo "" >> "$REPORT_FILE"
fi

echo "$REPORT_BLOCK" >> "$REPORT_FILE"

echo "📄 Validation report updated → context/_stage-validation.md"

STAGE_LOG="$BASE_DIR/information/_stage-registry.log"
TIMESTAMP=$(date "+%Y-%m-%d %H:%M")

echo "$TIMESTAMP | UNKNOWN | $(git branch --show-current) | VALIDATED | validate-stage.sh" >> "$STAGE_LOG"

exit "$EXIT_CODE"