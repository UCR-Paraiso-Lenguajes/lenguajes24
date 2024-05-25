# Usa una imagen de node como base 
FROM node:latest 

RUN apt-get update && apt-get install -y dos2unix

WORKDIR /app

COPY package*.json ./

RUN npm install
 
COPY . ./  
RUN chmod +x write_env.sh
 
EXPOSE 3000 
#CMD npm run dev
#CMD npm run
RUN dos2unix write_env.sh && chmod +x write_env.sh
CMD ./write_env.sh && npm run dev

#docker build -t alvaroui11 .
#docker run -d -e NODE_ENV=http://google.com  -p 3000:3000 alvaroui11