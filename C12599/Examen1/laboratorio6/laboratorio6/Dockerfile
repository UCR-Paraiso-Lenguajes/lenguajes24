# Usa una imagen de node como base 
FROM node:latest 
 
# Establece el directorio de trabajo dentro del contenedor 
WORKDIR /app 

COPY package.json ./
RUN npm install
 
# Copia los archivos del proyecto al contenedor 
COPY . . 

run npm run build 
 
WORKDIR /app/my-nextjs-app 
 
# Instala las dependencias del proyecto 
RUN npm install 
 
EXPOSE 3000 
CMD npm run dev 