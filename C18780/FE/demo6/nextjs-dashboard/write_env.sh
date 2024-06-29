#!/bin/bash

# Check if environmentUrl is set
if [ -z "$environmentUrl" ]; then
  echo "Error: environmentUrl is not set."
  exit 1
fi

# Write the environment variable to the .env file
echo "NEXT_PUBLIC_NODE_ENV=$environmentUrl" > .env

echo ".env file updated with NEXT_PUBLIC_NODE_ENV=$environmentUrl"
