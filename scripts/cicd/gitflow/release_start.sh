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
echo "Starting release: $RELEASE_VERSION"

git checkout develop
git pull

git flow release start $RELEASE_VERSION
git flow release publish $RELEASE_VERSION
