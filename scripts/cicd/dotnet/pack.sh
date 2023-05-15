#!/usr/bin/env bash
set -exo pipefail

# get full path to current directory
ROOT_DIR=$(pwd)
mkdir -p $ROOT_DIR/dist/Integration.Banjo.Base.Package

pushd infra/src/Base
PATCH_NUMBER=${BUILD_BUILDID:-0}
# get ado staging path
OUTPUT_PATH=${BUILD_ARTIFACTSTAGINGDIRECTORY:-$ROOT_DIR/dist}
dotnet pack -c Release -o $OUTPUT_PATH/Integration.Banjo.Base.Package -p:PackageVersion=1.1."$PATCH_NUMBER" -p:PackageID="Integration.Banjo.Base"
popd