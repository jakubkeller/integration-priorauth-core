# Ensure that we are connected to banjo sandbox
sandbox:
    ./scripts/cicd/infra/ensure_banjo_sandbox.sh

# grant execute permission to all shell scripts in scripts folder
chmod:
    find ./scripts -type f -name "*.sh" -exec chmod +x {} \;

synth: sandbox
    #!/usr/bin/env bash
    set -euxo pipefail
    export LOCAL_PATH=$(pwd)
    export ARTIFACTSTAGING_PATH=$(pwd)
    ./scripts/cicd/infra/synth.sh

clearcache:
    dotnet nuget locals all --clear

deploy: sandbox
    #!/usr/bin/env bash
    set -euxo pipefail
    export AWS_DEFAULT_REGION=us-east-2
    export LOCAL_PATH=$(pwd)
    export ARTIFACTSTAGING_PATH=$(pwd)
    ./scripts/cicd/infra/deploy.sh

cicd: sandbox
    #!/usr/bin/env bash
    set -euxo pipefail
    export AWS_DEFAULT_REGION=us-east-2
    export LOCAL_PATH=$(pwd)
    export ARTIFACTSTAGING_PATH=$(pwd)
    ./scripts/cicd/infra/synth.sh
    ./scripts/cicd/infra/deploy.sh
