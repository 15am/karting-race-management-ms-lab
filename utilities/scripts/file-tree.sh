#!/usr/bin/env bash
# 
# how to use: ./file-tree.sh "directories" depth
# example: ./file-tree.sh "src plan tasks" 2

TARGETS="${1:-src test deployment plan tasks}"
DEPTH="${2:-3}"

echo "📂 Project Structure"
echo "Targets: $TARGETS | Max Depth: $DEPTH"
echo "------------------------------------------"

if command -v tree >/dev/null 2>&1; then
    # Option A: Use tree if available
    tree -L "$DEPTH" $TARGETS
else
    # Option B: Universal fallback (find + sed)
    for dir in $TARGETS; do
        if [ -d "$dir" ]; then
            echo "$dir/"
            find "$dir" -maxdepth "$DEPTH" -not -path '*/.*' | sed -e "s/[^-][^\/]*\// |/g" -e "s/|\([^ ]\)/|-- \1/"
        else
            echo " [!] Directorio no encontrado: $dir"
        fi
    done
fi