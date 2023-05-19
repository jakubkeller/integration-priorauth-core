#!/bin/bash
# For an overview of set command options see https://www.gnu.org/software/bash/manual/html_node/The-Set-Builtin.html
# Navitus DevOps team does not recommend using -u as, in combination with -e it will cause references to optional parameters to fail the pipeline
# set -exo pipefail
set -eo pipefail

# variables
profileName="pod0-admin"
export DOTNET_SCRIPTS_PATH=$(pwd)/dotnet
export INFRA_SCRIPTS_PATH=$(pwd)/infra

source $INFRA_SCRIPTS_PATH/setenv.sh $profileName
chmod +x $DOTNET_SCRIPTS_PATH/*.sh
chmod +x $INFRA_SCRIPTS_PATH/*.sh
read -p "Local environment variables set and scripts made executable. Press enter to resume ..."

# install cdk
command -v cdk --version || npm install -g aws-cdk

# Login
aws sso login --profile $profileName
read -p "Login Complete. Press enter to resume ..."

# build
$DOTNET_SCRIPTS_PATH/build.sh
read -p "Build Complete. Press enter to resume ..."

# Publish 
$DOTNET_SCRIPTS_PATH/publish_app.sh
read -p "Publish Complete. Press enter to resume ..."

# Test
mkdir -p $TESTRESULT_PATH
$DOTNET_SCRIPTS_PATH/tests.sh
read -p "Test Complete. Press enter to resume ..."

# Cdk Diff
mkdir -p $ARTIFACTSTAGING_PATH
$INFRA_SCRIPTS_PATH/cdk_diff.sh $profileName
read -p "CDK Diff Complete. Press enter to resume ..."

# Synth
$INFRA_SCRIPTS_PATH/synth.sh $profileName
read -p "Synth Complete. Press enter to resume ..."

# Deploy
$INFRA_SCRIPTS_PATH/deploy.sh $profileName
read -p "Deploy Complete. Press enter to resume ..."

echo "Local Dev Build, publish, Test, synth, deploy completed."