# Hotel Booking App

A modern React application for hotel bookings built with TypeScript and Material UI.

## Features

- Browse available hotels
- Search hotels by location and dates
- View detailed hotel information
- Book rooms
- Manage bookings
- Responsive design
- Modern UI with Material UI components

## Technologies Used

- React 18
- TypeScript
- Material UI
- React Router
- Axios
- Date-fns

## Getting Started

### Prerequisites

- Node.js (v14 or higher)
- npm or yarn

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/hotel-booking-app.git
cd hotel-booking-app
```

2. Install dependencies:
```bash
npm install
```

3. Create a `.env` file in the root directory and add your environment variables:
```
REACT_APP_API_BASE_URL=your_api_url
```

4. Start the development server:
```bash
npm start
```

The application will be available at `http://localhost:3000`.

### Building for Production

To create a production build:

```bash
npm run build
```

The build files will be created in the `build` directory.

## Project Structure

```
src/
  ├── components/     # Reusable components
  ├── pages/         # Page components
  ├── services/      # API services
  ├── types/         # TypeScript type definitions
  ├── App.tsx        # Main application component
  └── index.tsx      # Application entry point
```

## API Integration

The application is designed to work with a RESTful API. The API endpoints are:

- `GET /hotels` - Get all hotels
- `GET /hotels/:id` - Get hotel details
- `GET /hotels/:id/rooms` - Get hotel rooms
- `POST /bookings` - Create a booking
- `GET /bookings/my` - Get user's bookings
- `POST /bookings/:id/cancel` - Cancel a booking

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
