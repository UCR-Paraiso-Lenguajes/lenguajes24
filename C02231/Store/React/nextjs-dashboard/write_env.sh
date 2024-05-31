#!/bin/bash

# Check if NEXT_PUBLIC_API_URL is set
if [ -z "$API_URL" ]; then
  echo "Error: NEXT_PUBLIC_NODE_ENV is not set."
fi

# Write the environment variable to the .env file
echo "NEXT_PUBLIC_API_URL=$API_URL" > .env

echo ".env file updated with NEXT_PUBLIC_API_URL=$API_URL"