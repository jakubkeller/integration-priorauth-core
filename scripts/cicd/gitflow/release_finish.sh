#!/bin/bash
set -exo pipefail
if [ -z "$1" ]
  then
    echo "Release version was not specified"
    exit 1
fi
# RELEASE_BRANCH=$1
# echo $RELEASE_BRANCH
# RELEASE_VERSION=${RELEASE_BRANCH##*/}
RELEASE_VERSION=$1
echo "Closing out release: $RELEASE_VERSION"

git checkout main
git pull
git checkout develop
git pull

export GIT_MERGE_AUTOEDIT=no

git flow release finish -m "Release" $RELEASE_VERSION

unset GIT_MERGE_AUTOEDIT

git checkout main
git push
git push --tags
git checkout develop
git push