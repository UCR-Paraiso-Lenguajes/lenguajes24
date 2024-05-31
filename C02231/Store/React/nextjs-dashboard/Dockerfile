# Usa una imagen de node como base 
FROM node:latest 

RUN apt-get update && apt-get install -y dos2unix
# Establece el directorio de trabajo dentro del contenedor 
WORKDIR /app 

# Copia los archivos del proyecto al contenedor 
COPY package*.json ./
# Copia los archivos del proyecto al contenedor 
COPY . . 
 
WORKDIR /app/my-nextjs-app 
 
# Instala las dependencias del proyecto 
RUN npm install 
 
COPY . ./

RUN chmod +x write_env.sh

EXPOSE 3000 

RUN dos2unix write_env.sh && chmod +x write_env.sh
CMD ./write_env.sh && npm run dev
#CMD npm run dev
 
# Compila la aplicación 
#RUN npm run dev 
 
# Expone el puerto en el que la aplicación se ejecutará 
#EXPOSE 3000 
 
# Comando para ejecutar la aplicación cuando se inicie el contenedor 
#CMD ["npm", "start"]docker build -t my-nextjs-app .


#docker build -t ejemplo .
#docker run -d -e NODE_ENV=http://google.com  -p 3000:3000 ejemplo11