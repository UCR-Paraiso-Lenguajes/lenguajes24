# Usa una imagen de node como base
FROM node:latest 

# Instala dos2unix
RUN apt-get update && apt-get install -y dos2unix

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia el package.json y package-lock.json primero, para aprovechar el cache de Docker
COPY package*.json ./

# Instala las dependencias
RUN npm install

# Copia el resto de los archivos del proyecto
COPY . .

# Establece el directorio de trabajo dentro de la aplicación Next.js
WORKDIR /app/my-nextjs-app

# Ejecuta el script de configuración de entorno y compila la aplicación
RUN dos2unix write_env.sh && chmod +x write_env.sh
RUN ./write_env.sh

# Compila la aplicación
RUN npm run build

# Expone el puerto en el que la aplicación se ejecutará
EXPOSE 3000

# Comando para ejecutar la aplicación cuando se inicie el contenedor
CMD ["npm", "run", "dev"]
