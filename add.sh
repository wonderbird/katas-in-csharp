#!/bin/bash
#
# Add new kata
#
set -Eeufo pipefail

read -p "Title: " TITLE
read -p "Kyu: " KYU
read -p "Source URL: " SOURCE
read -p "Keywords (comma separated): " KEYWORDS

DIR=$(echo "$TITLE" | sed -e 's/\s//g')
LOGIC=${DIR}.Logic
TEST=${LOGIC}.Tests

echo Generating New Project
echo ======================
echo
echo Title: $TITLE
echo Kyu: $KYU
echo Source: $SOURCE
echo Keywords: $KEYWORDS
echo
echo Directory: $DIR
echo Logic: $DIR/$LOGIC/$LOGIC.csproj
echo Tests: $DIR/$TEST/$TEST.csproj
echo

read -p "Press ENTER to continue" ENTER

mkdir "$DIR"
pushd "$DIR"
dotnet new classlib -o "$LOGIC"
dotnet new xunit -o "$TEST"
dotnet add "./$TEST/$TEST.csproj" reference "./$LOGIC/$LOGIC.csproj"

cat > README.md << EOF
---
title: $TITLE
kyu: $KYU
source: $SOURCE
keywords: $KEYWORDS
---

## $TITLE

TODO: Describe the kata inside this directory.
EOF

popd

dotnet sln katas-in-csharp.sln add "$DIR/$LOGIC/$LOGIC.csproj" --solution-folder "$DIR"
dotnet sln katas-in-csharp.sln add "$DIR/$TEST/$TEST.csproj" --solution-folder "$DIR"
