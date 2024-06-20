#!/bin/bash

if [ -z "$NEXT_PUBLIC_API" ]; then
  echo "Error: NEXT_PUBLIC_API_URL is not set."
fi

# Escribe la variable de entorno en el archivo .env
echo "NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL" > .env

# Muestra un mensaje indicando que el archivo .env ha sido actualizado
echo ".env file updated with NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL"