#!/bin/bash
#
# Add new kata
#
# This script will ask you some questions about your kata and then generate
#
# - a folder for the kata
# - a library project for the logic of the kata
# - a xUnit test project with reference to the library project
# - a README.md inside the folder
#
# It will add the csproj files to the overall solution file katas-in-csharp.sln
# and it will update the `.netconfig` with the additional coverage report.
#
# NOTE:
# Please add the README.md file to your solution folder manually as an existing item.
# At the time of writing the `dotnet sln add` command didn't support non-project files.
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

echo "    report = $DIR/$TEST/TestResults/*.xml" >> .netconfig
