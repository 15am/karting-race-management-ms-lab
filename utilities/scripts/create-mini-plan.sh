#!/usr/bin/env bash
set -euo pipefail

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

TEMPLATE="$BASE_DIR/utilities/templates/mini-plan-template.md"
OUT_DIR="$BASE_DIR/plan/stages"

STAGE_NUMBER="${1:-}"
STAGE_NAME="${2:-}"
BRANCH_NAME="${3:-}"
STATUS="${4:-Ready}" # Draft | Ready | Active | Completed

FORCE=0
if [[ "${5:-}" == "--force" ]]; then
  FORCE=1
fi

if [[ -z "$STAGE_NUMBER" || -z "$STAGE_NAME" || -z "$BRANCH_NAME" ]]; then
  echo "Usage: create-mini-plan.sh <stage-number> <stage-name> <branch-name> [status] [--force]"
  echo "Example: create-mini-plan.sh 1 domain-foundation feature/domain-foundation Ready"
  exit 1
fi

if [[ ! -f "$TEMPLATE" ]]; then
  echo "Template not found: $TEMPLATE"
  exit 2
fi

mkdir -p "$OUT_DIR"

OUT_FILE="$OUT_DIR/mini-plan-stage-${STAGE_NUMBER}.md"

if [[ -f "$OUT_FILE" && "$FORCE" -ne 1 ]]; then
  echo "Mini plan already exists: $OUT_FILE"
  echo "Use --force to overwrite."
  exit 0
fi

cp "$TEMPLATE" "$OUT_FILE"

# Pre-fill key metadata lines if present in template.
# These replacements assume the optimized mini-plan template has these keys:
# StageNumber:, StageName:, BranchName:, Status:
sed -i \
  -e "s/^StageNumber:.*$/StageNumber: ${STAGE_NUMBER}/" \
  -e "s/^StageName:.*$/StageName: ${STAGE_NAME}/" \
  -e "s|^BranchName:.*$|BranchName: ${BRANCH_NAME}|" \
  -e "s/^Status:.*$/Status: ${STATUS}/" \
  "$OUT_FILE" || true

echo "✔ Created mini plan → $OUT_FILE"