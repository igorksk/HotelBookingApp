import React, { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
  Container,
  Grid,
  Card,
  CardContent,
  Typography,
  Rating,
  Button,
  Box,
  CircularProgress,
  Alert,
} from '@mui/material';
import { HotelDto } from '../types/api.types';
import { hotelsApi } from '../services/api';

const HotelList = () => {
  const [hotels, setHotels] = useState<HotelDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchHotels = async () => {
      try {
        setLoading(true);
        setError(null);
        const location = searchParams.get('location');
        const checkIn = searchParams.get('checkIn');
        const checkOut = searchParams.get('checkOut');
        
        const params = {
          ...(location && { location }),
          ...(checkIn && { checkIn }),
          ...(checkOut && { checkOut }),
        };

        const data = await hotelsApi.getAll(params);
        setHotels(data);
      } catch (err) {
        setError('Failed to load hotels. Please try again later.');
        console.error('Error fetching hotels:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchHotels();
  }, [searchParams]);

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
      <Typography variant="h4" component="h1" sx={{ mb: 4 }}>
        Available Hotels
      </Typography>

      {hotels.length === 0 ? (
        <Typography variant="body1" color="text.secondary">
          No hotels found matching your criteria.
        </Typography>
      ) : (
        <Grid container spacing={4}>
          {hotels.map((hotel) => (
            <Box key={hotel.id} sx={{ width: { xs: '100%', md: '50%', lg: '33.33%' }, p: 1 }}>
              <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                <CardContent sx={{ flexGrow: 1 }}>
                  <Typography gutterBottom variant="h5" component="h2">
                    {hotel.name || 'Unnamed Hotel'}
                  </Typography>
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                    <Rating value={hotel.rating} precision={0.5} readOnly />
                    <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                      ({hotel.rating})
                    </Typography>
                  </Box>
                  <Typography variant="body2" color="text.secondary" paragraph>
                    {hotel.address || 'Address not specified'}
                  </Typography>
                  <Typography variant="body2" color="text.secondary" paragraph>
                    {hotel.cityName}, {hotel.countryName}
                  </Typography>
                  {hotel.rooms && hotel.rooms.length > 0 && (
                    <>
                      <Typography variant="h6" color="primary" paragraph>
                        From ${Math.min(...hotel.rooms.map(room => room.pricePerNight))} / night
                      </Typography>
                      <Button
                        variant="contained"
                        fullWidth
                        onClick={() => {
                          if (hotel.rooms && hotel.rooms.length > 0) {
                            navigate(`/booking/${hotel.id}/${hotel.rooms[0].id}`);
                          }
                        }}
                      >
                        Book Now
                      </Button>
                    </>
                  )}
                </CardContent>
              </Card>
            </Box>
          ))}
        </Grid>
      )}
    </Container>
  );
};

export default HotelList; 