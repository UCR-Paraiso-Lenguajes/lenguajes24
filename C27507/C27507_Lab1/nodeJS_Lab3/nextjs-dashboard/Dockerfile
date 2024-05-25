# Usa una imagen de node como base 
FROM node:latest 
 
WORKDIR /react_app 

COPY package*.json ./

RUN npm install
 
COPY . ./  
 
EXPOSE 3000 
CMD npm run dev 
 
# Compila la aplicación 
#RUN npm run dev 
 
# Expone el puerto en el que la aplicación se ejecutará 
#EXPOSE 3000 
 
# Comando para ejecutar la aplicación cuando se inicie el contenedor 
#CMD ["npm", "start"]docker build -t react-app .