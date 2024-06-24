FROM node:latest

RUN apt-get update && apt-get install -y dos2unix

WORKDIR /app/my-nextjs-app

COPY my-nextjs-app/package*.json ./

RUN npm install

COPY ./my-nextjs-app ./ 
COPY ./write_env.sh ./ 

RUN chmod +x write_env.sh

EXPOSE 3000
RUN npm run build

RUN dos2unix write_env.sh && chmod +x write_env.sh
CMD ./write_env.sh && npm start

#docker build -t alvaroui11 .
#docker run -d -e NODE_ENV=http://google.com  -p 3000:3000 alvaroui11
