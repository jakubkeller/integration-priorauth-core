#!/bin/bash
#########################################################
# Purpose: Ensure that the AWS account is POD16 - PriorAuth Sandbox
#########################################################

aws_account_id=$(aws sts get-caller-identity --query Account --output text)

# if aws_account_id is not 123 then exit 1
if [ $aws_account_id != 255169513165 ]; then
    echo "AWS Account ID is not POD16 - PriorAuth Sandbox"
    exit 1
fi
echo "AWS Account is POD16 - PriorAuth Sandbox"


