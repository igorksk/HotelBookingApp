FROM node:18-alpine

WORKDIR /app

COPY hotel-booking-app/package*.json ./

RUN npm install

COPY hotel-booking-app/ .

RUN npm run build

EXPOSE 3000

CMD ["npm", "start"] 