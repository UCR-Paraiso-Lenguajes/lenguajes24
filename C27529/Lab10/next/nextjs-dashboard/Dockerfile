# Usa una imagen de node como base
FROM node:latest 

# Instala dos2unix
RUN apt-get update && apt-get install -y dos2unix

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia el package.json y package-lock.json primero, para aprovechar el cache de Docker
COPY package.json ./

RUN npm install
#RUN npm install chart.js react-chartjs-2 react-google-charts react-modal react-data-table-component

# Copia el resto de los archivos del proyecto

#COPY . .

WORKDIR /app/my-nextjs-app


COPY . .
 
# Compila la aplicación
#RUN npm run build
RUN chmod +x write_env.sh
# Expone el puerto en el que la aplicación se ejecutará
EXPOSE 3000

RUN dos2unix write_env.sh && chmod +x write_env.sh

# Comando para ejecutar la aplicación cuando se inicie el contenedor
CMD  ./write_env.sh && npm run dev
