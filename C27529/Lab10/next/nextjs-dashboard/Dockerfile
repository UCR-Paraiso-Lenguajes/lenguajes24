# Usa una imagen de node como base
FROM node:latest 

# Establece el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copia el package.json y package-lock.json primero, para aprovechar el cache de Docker
COPY package*.json ./

RUN npm install chart.js react-chartjs-2 react-google-charts react-modal react-data-table-component

# Copia el resto de los archivos del proyecto
COPY . .

WORKDIR /app/my-nextjs-app

# Instala las dependencias del proyecto
RUN npm install

COPY . .
 
# Compila la aplicación
#RUN npm run build
RUN chmod +x write_env.sh
# Expone el puerto en el que la aplicación se ejecutará
EXPOSE 3000

RUN dos2unix write_env.sh && chmod +x write_env.sh
CMD ./write_env.sh && npm run dev


# Comando para ejecutar la aplicación cuando se inicie el contenedor
CMD ["npm", "run", "dev"]