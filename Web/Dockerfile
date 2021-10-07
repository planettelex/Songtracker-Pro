FROM node:latest as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY ./ .
RUN npm run build

FROM steebchen/nginx-spa:stable as final
WORKDIR /app
COPY --from=build-stage /app/dist /app

EXPOSE 80

CMD ["nginx"]
