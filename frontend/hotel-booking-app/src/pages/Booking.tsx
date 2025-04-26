import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  TextField,
  Button,
  CircularProgress,
  Alert,
  Paper,
} from '@mui/material';
import type { Booking } from '../types/api.types';
import { HotelDto, RoomDto } from '../types/api.types';
import { hotelsApi, bookingsApi } from '../services/api';

const BookingPage = () => {
  const { hotelId, roomId } = useParams<{ hotelId: string; roomId: string }>();
  const navigate = useNavigate();
  const [hotel, setHotel] = useState<HotelDto | null>(null);
  const [room, setRoom] = useState<RoomDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState({
    guestName: '',
    guestEmail: '',
    checkInDate: '',
    checkOutDate: '',
  });

  useEffect(() => {
    const fetchBookingDetails = async () => {
      if (!hotelId || !roomId) {
        setError('Invalid hotel or room ID');
        setLoading(false);
        return;
      }

      try {
        const [hotelData, roomData] = await Promise.all([
          hotelsApi.getById(Number(hotelId)),
          hotelsApi.getRooms(Number(hotelId)).then(rooms => 
            rooms.find(room => room.id === Number(roomId))
          ),
        ]);

        if (!hotelData || !roomData) {
          setError('Hotel or room not found');
          setLoading(false);
          return;
        }

        setHotel(hotelData);
        setRoom(roomData);
      } catch (err) {
        setError('Failed to load booking details. Please try again later.');
        console.error('Error fetching booking details:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchBookingDetails();
  }, [hotelId, roomId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!hotelId || !roomId) return;

    try {
      const booking: Omit<Booking, 'id' | 'room' | 'totalPrice'> = {
        guestName: formData.guestName,
        guestEmail: formData.guestEmail,
        checkInDate: formData.checkInDate,
        checkOutDate: formData.checkOutDate,
        roomId: Number(roomId),
        status: null
      };

      await bookingsApi.create(booking);
      navigate('/bookings');
    } catch (err) {
      setError('Failed to create booking. Please try again later.');
      console.error('Error creating booking:', err);
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  if (!hotel || !room) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Alert severity="error">Hotel or room not found</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      <Typography variant="h4" component="h1" sx={{ mb: 4 }}>
        Book a Room
      </Typography>

      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          {hotel.name}
        </Typography>
        <Typography variant="body1" color="text.secondary" paragraph>
          {hotel.address}
        </Typography>
        <Typography variant="body1" color="text.secondary" paragraph>
          {hotel.cityName}, {hotel.countryName}
        </Typography>

        <Box sx={{ mt: 4 }}>
          <Typography variant="h6" gutterBottom>
            Room Details
          </Typography>
          <Typography variant="body1" paragraph>
            Room Number: {room.roomNumber}
          </Typography>
          <Typography variant="body1" paragraph>
            Type: {room.type}
          </Typography>
          <Typography variant="body1" paragraph>
            Price per Night: ${room.pricePerNight}
          </Typography>
        </Box>

        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 4 }}>
          <TextField
            fullWidth
            label="Guest Name"
            value={formData.guestName}
            onChange={(e) => setFormData({ ...formData, guestName: e.target.value })}
            required
            margin="normal"
          />
          <TextField
            fullWidth
            label="Guest Email"
            type="email"
            value={formData.guestEmail}
            onChange={(e) => setFormData({ ...formData, guestEmail: e.target.value })}
            required
            margin="normal"
          />
          <TextField
            fullWidth
            label="Check-in Date"
            type="date"
            value={formData.checkInDate}
            onChange={(e) => setFormData({ ...formData, checkInDate: e.target.value })}
            required
            margin="normal"
            InputLabelProps={{ shrink: true }}
          />
          <TextField
            fullWidth
            label="Check-out Date"
            type="date"
            value={formData.checkOutDate}
            onChange={(e) => setFormData({ ...formData, checkOutDate: e.target.value })}
            required
            margin="normal"
            InputLabelProps={{ shrink: true }}
          />

          <Button
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            sx={{ mt: 3 }}
          >
            Confirm Booking
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default BookingPage; 