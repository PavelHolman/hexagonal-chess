#!/usr/bin/env bash
# Build the WebGL version and commit it to the gh-pages branch (push separately).
set -euo pipefail

cd "$(dirname "$0")/.."
UNITY="${UNITY:-/Applications/Unity/Hub/Editor/6000.6.10f1/Unity.app/Contents/MacOS/Unity}"

"$UNITY" -batchmode -quit -projectPath "$PWD" -buildTarget WebGL \
  -executeMethod BuildScript.BuildWebGL -logFile Logs/webgl-build.log

worktree="$(mktemp -d)"
trap 'git worktree remove --force "$worktree"' EXIT

if git show-ref --quiet refs/heads/gh-pages; then
  git worktree add "$worktree" gh-pages
else
  git worktree add --orphan -b gh-pages "$worktree"
fi

rsync -a --delete --exclude .git Builds/WebGL/ "$worktree/"
touch "$worktree/.nojekyll"

git -C "$worktree" add -A
if git -C "$worktree" diff --cached --quiet; then
  echo "gh-pages is already up to date"
else
  git -C "$worktree" commit -m "Deploy WebGL build from $(git rev-parse --short HEAD)"
  echo "Committed to gh-pages. Push with: git push origin gh-pages"
fi
