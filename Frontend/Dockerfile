# Stage 1: Build the application
FROM node:14-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build

# Stage 2: Use an official Nginx image as a base image
FROM nginx:alpine

# Copy your custom Nginx configuration file
COPY default.conf /etc/nginx/conf.d/default.conf

# Copy static files (assuming your Vite build output is in /app/dist)
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the port used by Nginx
EXPOSE 80

# Start Nginx in the foreground
CMD ["nginx", "-g", "daemon off;"]
