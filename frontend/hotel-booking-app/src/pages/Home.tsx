import React from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Typography,
  Box,
  Paper,
  TextField,
  Button,
  Container,
  Grid,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';

const Home = () => {
  const navigate = useNavigate();
  const [checkIn, setCheckIn] = React.useState<Date | null>(null);
  const [checkOut, setCheckOut] = React.useState<Date | null>(null);
  const [country, setCountry] = React.useState('');
  const [city, setCity] = React.useState('');

  const handleSearch = () => {
    const params = new URLSearchParams();
    if (country) params.append('country', country);
    if (city) params.append('city', city);
    if (checkIn) params.append('checkIn', checkIn.toISOString());
    if (checkOut) params.append('checkOut', checkOut.toISOString());
    navigate(`/hotels?${params.toString()}`);
  };

  return (
    <Container maxWidth="lg">
      <Box sx={{ mt: 8, mb: 4 }}>
        <Typography variant="h2" component="h1" align="center" gutterBottom>
          Find Your Perfect Stay
        </Typography>
        <Typography variant="h5" align="center" color="text.secondary" paragraph>
          Book hotels and rooms at the best prices
        </Typography>
      </Box>

      <Paper elevation={3} sx={{ p: 4, mt: 4 }}>
        <Grid container spacing={3}>
          <Box sx={{ width: { xs: '100%', md: '25%' }, p: 1 }}>
            <TextField
              fullWidth
              label="Country"
              value={country}
              onChange={(e) => setCountry(e.target.value)}
              placeholder="Enter country"
            />
          </Box>
          <Box sx={{ width: { xs: '100%', md: '25%' }, p: 1 }}>
            <TextField
              fullWidth
              label="City"
              value={city}
              onChange={(e) => setCity(e.target.value)}
              placeholder="Enter city"
            />
          </Box>
          <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box sx={{ width: { xs: '100%', md: '20%' }, p: 1 }}>
              <DatePicker
                label="Check-in"
                value={checkIn}
                onChange={(newValue) => setCheckIn(newValue)}
              />
            </Box>
            <Box sx={{ width: { xs: '100%', md: '20%' }, p: 1 }}>
              <DatePicker
                label="Check-out"
                value={checkOut}
                onChange={(newValue) => setCheckOut(newValue)}
              />
            </Box>
          </LocalizationProvider>
          <Box sx={{ width: { xs: '100%', md: '10%' }, p: 1 }}>
            <Button
              fullWidth
              variant="contained"
              size="large"
              onClick={handleSearch}
              sx={{ height: '100%' }}
            >
              Search
            </Button>
          </Box>
        </Grid>
      </Paper>
    </Container>
  );
};

export default Home; 