#!/usr/bin/env bash
set -euo pipefail

BASE_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
SCRIPTS="$BASE_DIR/utilities/scripts"

STAGE_COUNT="${1:-}"

if [[ -z "$STAGE_COUNT" ]]; then
  echo "Usage: bootstrap-plans.sh <stage-count>"
  echo "Example: bootstrap-plans.sh 4"
  exit 1
fi

"$SCRIPTS/create-master-plan.sh"

for i in $(seq 1 "$STAGE_COUNT"); do
  # Default placeholders; Planner edits later.
  "$SCRIPTS/create-mini-plan.sh" "$i" "stage-${i}-name" "feature/stage-${i}-name" "Draft"
done

echo "✔ Bootstrapped plans (master + ${STAGE_COUNT} mini plans)."