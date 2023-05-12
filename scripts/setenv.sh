#!/bin/bash
set -eo pipefail

if [ -n "$NODE_VERSION" ]; then
    echo "Setting nvm to use $NODE_VERSION"
    source ~/bin/nvm/nvm.sh
    nvm use --silent $NODE_VERSION || nvm install $NODE_VERSION --latest-npm
else #We will load latest version of node by default
    echo "Use latest version of node"
    source ~/bin/nvm/nvm.sh
    nvm use --silent --lts || nvm install --lts --latest-npm
fi

if [ -n "$CDK_VERSION" ]; then
    echo "Setting cdk to use $CDK_VERSION"
    npm install --location=global aws-cdk@$CDK_VERSION
else
    echo "Setting cdk to use latest version"
    npm install --location=global aws-cdk #ensure latest version of aws-cdk is installed under version of node just set
fi
echo "aws cdk version being used: $(npx cdk --version)"

if [ -n "$PYTHON_VERSION" ]; then
    echo "CI/CD Pipeline - Use pyenv to set use python $PYTHON_VERSION for all directories under $(pwd)"
    eval "$(pyenv init -)"
    eval "$(pyenv virtualenv-init -)"
    pyenv local $PYTHON_VERSION 2>&- || eval "pyenv install -s $PYTHON_VERSION; pyenv local $PYTHON_VERSION"
    pyenv virtualenv $PYTHON_VERSION pharmacy_venv || echo "Pyenv virtual env already exists"
    pyenv activate pharmacy_venv
    python -V
    python -m pip install --upgrade pip
    pip install virtualenv
    python -m virtualenv .venv
    pyenv deactivate
    source .venv/bin/activate
    python -V
fi