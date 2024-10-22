# Stage 1: Build the Angular app
FROM node:20-alpine AS build

WORKDIR /app

COPY package.json package-lock.json nginx.conf ./
RUN npm install

COPY . .
RUN npm run container-build

# Stage 2: Serve the app with Nginx
FROM nginx:alpine

COPY --from=build /app/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/gameoflifeui/browser /usr/share/nginx/html

EXPOSE 80