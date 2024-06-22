FROM node:latest 
 
RUN apt-get update && apt-get install -y dos2unix

WORKDIR /app 
 
COPY . . 

RUN npm install
  
RUN chmod +x write_env.sh
 
EXPOSE 3000

RUN dos2unix write_env.sh && chmod +x write_env.sh
CMD ./write_env.sh && npm run dev
