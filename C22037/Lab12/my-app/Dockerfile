# Usa una imagen base oficial de Node.js
FROM node:latest

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /my-app

# Copia el archivo package.json y package-lock.json
COPY package*.json ./

# Instala las dependencias del proyecto
RUN npm install jwt-decode --save
RUN npm i --save-dev @types/react-datepicker
RUN npm install

# Copia el resto del código de la aplicación
COPY . .

# Construye la aplicación Next.js
#RUN npm run build

# Exponer el puerto en el que se ejecutará la aplicación
EXPOSE 3000

# Comando por defecto al iniciar el contenedor
CMD ["npm", "run", "dev"]