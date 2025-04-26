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
  const [location, setLocation] = React.useState('');

  const handleSearch = () => {
    const params = new URLSearchParams();
    if (location) params.append('location', location);
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
          <Box sx={{ width: { xs: '100%', md: '33.33%' }, p: 1 }}>
            <TextField
              fullWidth
              label="Location"
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              placeholder="Where are you going?"
            />
          </Box>
          <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box sx={{ width: { xs: '100%', md: '25%' }, p: 1 }}>
              <DatePicker
                label="Check-in"
                value={checkIn}
                onChange={(newValue) => setCheckIn(newValue)}
              />
            </Box>
            <Box sx={{ width: { xs: '100%', md: '25%' }, p: 1 }}>
              <DatePicker
                label="Check-out"
                value={checkOut}
                onChange={(newValue) => setCheckOut(newValue)}
              />
            </Box>
          </LocalizationProvider>
          <Box sx={{ width: { xs: '100%', md: '16.67%' }, p: 1 }}>
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