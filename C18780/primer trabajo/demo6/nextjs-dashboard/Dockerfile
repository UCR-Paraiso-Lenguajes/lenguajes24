# Usa una imagen de node como base 
FROM node:latest 
 
# Establece el directorio de trabajo dentro del contenedor 
WORKDIR /app 
 
# Copia los archivos del proyecto al contenedor 
COPY . . 
 
WORKDIR /app 
 
# Instala las dependencias del proyecto 
RUN npm install 
 
EXPOSE 3000 
CMD npm run dev 