#!/usr/bin/env bash
set -euo pipefail

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

TEMPLATE="$BASE_DIR/utilities/templates/master-plan-template.md"
OUT_DIR="$BASE_DIR/plan"
OUT_FILE="$OUT_DIR/master-plan.md"

FORCE=0
if [[ "${1:-}" == "--force" ]]; then
  FORCE=1
fi

if [[ ! -f "$TEMPLATE" ]]; then
  echo "Template not found: $TEMPLATE"
  exit 2
fi

mkdir -p "$OUT_DIR"

if [[ -f "$OUT_FILE" && "$FORCE" -ne 1 ]]; then
  echo "Master plan already exists: $OUT_FILE"
  echo "Use --force to overwrite."
  exit 0
fi

cp "$TEMPLATE" "$OUT_FILE"
echo "✔ Created master plan → $OUT_FILE"