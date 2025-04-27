import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Typography,
  Box,
  Paper,
  TextField,
  Button,
  Container,
  Grid,
  Autocomplete,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { locationsApi } from '../services/api';

const Home = () => {
  const navigate = useNavigate();
  const [checkIn, setCheckIn] = React.useState<Date | null>(null);
  const [checkOut, setCheckOut] = React.useState<Date | null>(null);
  const [country, setCountry] = React.useState<string>('');
  const [city, setCity] = React.useState<string>('');
  const [countries, setCountries] = useState<string[]>([]);
  const [cities, setCities] = useState<string[]>([]);
  const [loadingCountries, setLoadingCountries] = useState(false);
  const [loadingCities, setLoadingCities] = useState(false);

  useEffect(() => {
    const fetchCountries = async () => {
      try {
        setLoadingCountries(true);
        const data = await locationsApi.getCountries();
        setCountries(data);
      } catch (error) {
        console.error('Error fetching countries:', error);
      } finally {
        setLoadingCountries(false);
      }
    };

    fetchCountries();
  }, []);

  useEffect(() => {
    const fetchCities = async () => {
      if (!country) {
        setCities([]);
        return;
      }

      try {
        setLoadingCities(true);
        const data = await locationsApi.getCities(country);
        setCities(data);
      } catch (error) {
        console.error('Error fetching cities:', error);
      } finally {
        setLoadingCities(false);
      }
    };

    fetchCities();
  }, [country]);

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
            <Autocomplete
              options={countries}
              loading={loadingCountries}
              value={country}
              onChange={(_, newValue) => setCountry(newValue || '')}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label="Country"
                  placeholder="Select country"
                />
              )}
            />
          </Box>
          <Box sx={{ width: { xs: '100%', md: '25%' }, p: 1 }}>
            <Autocomplete
              options={cities}
              loading={loadingCities}
              value={city}
              onChange={(_, newValue) => setCity(newValue || '')}
              disabled={!country}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label="City"
                  placeholder="Select city"
                />
              )}
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