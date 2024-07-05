#!/bin/bash

# Check if NEXT_PUBLIC_NODE_ENV is set
if [ -z "$NODE_ENV" ]; then
  echo "Error: NEXT_PUBLIC_NODE_ENV is not set."
fi

# Write the environment variable to the .env file
echo "NEXT_PUBLIC_NODE_ENV=$NODE_ENV" > .env

# Write the environment variable to the .env.production file
echo "NEXT_PUBLIC_NODE_ENV=$NODE_ENV" > .env.production

echo ".env file updated with NEXT_PUBLIC_NODE_ENV=$NODE_ENV"
