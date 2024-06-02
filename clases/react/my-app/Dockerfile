# Use an official Node.js image as the base image
FROM node:latest AS build

# Set the working directory in the container
WORKDIR /app

# Copy the package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . .

# Build the Next.js application
RUN npm run build

# Expose the port on which the Next.js application runs
EXPOSE 3000

# Define the command to start the Next.js application
CMD ["npm", "run", "dev"]