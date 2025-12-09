import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Card,
  CardContent,
  Button,
  Box,
  CircularProgress,
  Alert,
  Chip,
} from '@mui/material';
import { BookingDto } from '../types/api.types';
import { bookingsApi } from '../services/api';

const MyBookings = () => {
  const [bookings, setBookings] = useState<BookingDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [cancelError, setCancelError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchBookings = async () => {
      try {
        setLoading(true);
        setError(null);
        // TODO: Replace with actual user ID
        const data = await bookingsApi.getByUser(1);
        setBookings(data);
      } catch (err) {
        setError('Failed to load bookings. Please try again later.');
        console.error('Error fetching bookings:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchBookings();
  }, []);

  const handleCancelBooking = async (bookingId: number) => {
    setCancelError(null);
    try {
      await bookingsApi.cancel(bookingId);
      // Refresh bookings list
      const data = await bookingsApi.getByUser(1);
      setBookings(data);
    } catch (err) {
      setCancelError('Failed to cancel booking. Please try again later.');
      console.error('Error cancelling booking:', err);
    }
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg">
      <Typography variant="h4" component="h1" gutterBottom>
        My Bookings
      </Typography>

      {bookings.length === 0 ? (
        <Typography variant="body1" color="text.secondary">
          You have no bookings yet.
        </Typography>
      ) : (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
          {bookings.map((booking) => (
            <Card key={booking.id}>
              <CardContent>
                <Box sx={{ display: 'flex', flexDirection: { xs: 'column', md: 'row' }, gap: 2 }}>
                  <Box sx={{ flex: { xs: '1 1 100%', md: '2 1 0%' } }}>
                    <Typography variant="h6" gutterBottom>
                      {booking.hotelName || 'Unnamed Hotel'}
                    </Typography>
                    <Typography variant="body2" color="text.secondary" paragraph>
                      {booking.hotelAddress || 'Address not specified'}
                    </Typography>
                    <Typography variant="body2" color="text.secondary" paragraph>
                      {booking.hotelCity || 'City not specified'}, {booking.hotelCountry || 'Country not specified'}
                    </Typography>
                    <Typography variant="body2" color="text.secondary" paragraph>
                      Room: {booking.roomType || 'Standard Room'} ({booking.roomNumber || 'N/A'})
                    </Typography>
                    <Typography variant="body2" color="text.secondary" paragraph>
                      Price per night: ${booking.pricePerNight}
                    </Typography>
                    <Typography variant="body2" color="text.secondary" paragraph>
                      Check-in: {new Date(booking.checkInDate).toLocaleDateString()}
                    </Typography>
                    <Typography variant="body2" color="text.secondary" paragraph>
                      Check-out: {new Date(booking.checkOutDate).toLocaleDateString()}
                    </Typography>
                    <Typography variant="h6" color="primary" paragraph>
                      Total: ${booking.totalPrice}
                    </Typography>
                  </Box>
                  <Box sx={{ flex: { xs: '1 1 100%', md: '1 1 0%' } }}>
                    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end' }}>
                      <Chip
                        label={(booking.status || 'PENDING').toUpperCase()}
                        color={booking.status === 'Confirmed' ? 'success' : 'warning'}
                        sx={{ mb: 2 }}
                      />
                      <Button
                        variant="outlined"
                        color="error"
                        onClick={() => handleCancelBooking(booking.id)}
                      >
                        Cancel Booking
                      </Button>
                    </Box>
                  </Box>
                </Box>
              </CardContent>
            </Card>
          ))}
        </Box>
      )}
      {cancelError && (
        <Alert severity="error" sx={{ mt: 2 }}>
          {cancelError}
        </Alert>
      )}
    </Container>
  );
};

export default MyBookings; 