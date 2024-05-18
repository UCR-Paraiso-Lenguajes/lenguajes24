# Usa una imagen de node como base 
FROM node:latest 
 
# Establece el directorio de trabajo dentro del contenedor 
WORKDIR /my-app 
 
# Copia el package.json y package-lock.json primero, para aprovechar el cache de Docker 
COPY package*.json ./ 

RUN npm install --save bootstrap@^4.0.0-alpha.6  react-bootstrap@^0.32.1

# Instala las dependencias del proyecto 
RUN npm install 
 
# Copia el resto de los archivos del proyecto 
COPY . . 
 
# Establece el directorio de trabajo dentro del contenedor, donde está el proyecto de Next.js 
#WORKDIR /my-app/app 
  
EXPOSE 3000  
CMD npm run dev 
# Compila la aplicación 
#RUN npm run build 
 
# Expone el puerto en el que la aplicación se ejecutará 
#EXPOSE 3000 
 
# Comando para ejecutar la aplicación cuando se inicie el contenedor 
#CMD ["npm", "start"]