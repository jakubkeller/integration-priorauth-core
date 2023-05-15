#!/bin/bash
# For an overview of set command options see https://www.gnu.org/software/bash/manual/html_node/The-Set-Builtin.html
# Navitus DevOps team does not recommend using -u as, in combination with -e it will cause references to optional parameters to fail the pipeline
# set -exo pipefail
set -eo pipefail

profileName=${1:-""}

# NOTE: You may need to change these relative paths used below to match your repository structure

# Variables in ALLCAPS are environment variables defined within the vars/vars-[environment].yaml templates

echo ""
echo "setenv.sh"
echo ""
echo "Environment variables used:"
echo "================================"
echo "LOCAL_PATH: $LOCAL_PATH"
echo "NODE_VERSION: $NODE_VERSION"
echo "PYTHON_VERSION: $PYTHON_VERSION"
echo "CDK_VERSION: $CDK_VERSION"

if [ -n "$profileName" ]; then
    echo "Local Development - Profile $profileName specified. Setting local environment variables."
    export INFRA_SCRIPTS_PATH=$(pwd)
    export LOCAL_PATH=$(cd "$dir../../" && pwd)
    mkdir -p $LOCAL_PATH/dist/tests
    mkdir -p $LOCAL_PATH/dist/stage
    export BUILD_PATH="$LOCAL_PATH/dist"
    export PUBLISH_PATH="$LOCAL_PATH/dist"
    export TESTRESULT_PATH="$LOCAL_PATH/dist/tests"
    export ARTIFACTSTAGING_PATH="$LOCAL_PATH/dist/stage"
else
    if [ -n "$NODE_VERSION" ]; then
        echo "CI/CD Pipeline - No Profile Name - Setting nvm to use $NODE_VERSION"
        source ~/bin/nvm/nvm.sh
        nvm use --silent $NODE_VERSION || nvm install $NODE_VERSION --latest-npm
#        command -v cdk --version 1>&- 2>&- || npm install --location=global aws-cdk #ensure aws-cdk is installed under version of node just set
    else #We will load latest version of node by default
       echo "CI/CD Pipeline - No Profile Name - Use latest version of node"
        source ~/bin/nvm/nvm.sh
        nvm use --silent --lts || nvm install --lts --latest-npm
    fi

    if [ -n "$CDK_VERSION" ]; then
        npm install aws-cdk@$CDK_VERSION
    else
        npm install --location=global aws-cdk #ensure latest version of aws-cdk is installed under version of node just set
    fi
    echo "aws cdk version being used: $(npx cdk --version)"

    if [ -n "$PYTHON_VERSION" ]; then
        echo "CI/CD Pipeline - No Profile Name - use pyenv to set use python $PYTHON_VERSION for all directories under $(pwd)"
        eval "$(pyenv init -)"
        eval "$(pyenv virtualenv-init -)"
        pyenv local $PYTHON_VERSION 2>&- || eval "pyenv install -s $PYTHON_VERSION; pyenv local $PYTHON_VERSION"
        python -m pip install --upgrade pip setuptools virtualenv wheel
        python -m virtualenv .venv
    fi
fi