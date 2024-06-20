# Usa una imagen de node como base 
FROM node:latest 
 
# Establece el directorio de trabajo dentro del contenedor 
WORKDIR /app 
RUN apt-get update && apt-get install -y dos2unix
 
# Copia los archivos del proyecto al contenedor 
COPY . . 

COPY package*.json ./
 
WORKDIR /app

RUN chmod +x write_env.sh
 
# Instala las dependencias del proyecto 
RUN npm install 
 
EXPOSE 3000 
CMD npm run dev 
 
# Compila la aplicación 
#RUN npm run dev 
 
# Expone el puerto en el que la aplicación se ejecutará 
#EXPOSE 3000 
 
# Comando para ejecutar la aplicación cuando se inicie el contenedor 
#CMD ["npm", "start"]docker build -t my-nextjs-app .

RUN dos2unix write_env.sh && chmod +x write_env.sh
CMD ./write_env.sh && npm run dev