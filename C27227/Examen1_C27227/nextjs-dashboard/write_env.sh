#!/bin/bash

if [ -z "$NEXT_PUBLIC_API" ]; then
  echo "Error: NEXT_PUBLIC_API is not set."
fi

# Escribe la variable de entorno en el archivo .env
echo "NEXT_PUBLIC_API=$NEXT_PUBLIC_API" > .env

# Muestra un mensaje indicando que el archivo .env ha sido actualizado
echo ".env file updated with NEXT_PUBLIC_API=$NEXT_PUBLIC_API"