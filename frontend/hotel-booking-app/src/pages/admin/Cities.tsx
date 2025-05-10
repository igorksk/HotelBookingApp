import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Alert,
  Snackbar,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { citiesApi, countriesApi } from '../../services/api';
import { City, Country } from '../../types/api.types';

const Cities: React.FC = () => {
  const [cities, setCities] = useState<City[]>([]);
  const [countries, setCountries] = useState<Country[]>([]);
  const [open, setOpen] = useState(false);
  const [editingCity, setEditingCity] = useState<City | null>(null);
  const [formData, setFormData] = useState({ name: '', countryId: '' });
  const [error, setError] = useState<string | null>(null);

  const fetchCities = async () => {
    try {
      const data = await citiesApi.getAll();
      setCities(data);
    } catch (error) {
      console.error('Error fetching cities:', error);
      setError('Failed to fetch cities');
    }
  };

  const fetchCountries = async () => {
    try {
      const data = await countriesApi.getAll();
      setCountries(data);
    } catch (error) {
      console.error('Error fetching countries:', error);
      setError('Failed to fetch countries');
    }
  };

  useEffect(() => {
    fetchCities();
    fetchCountries();
  }, []);

  const handleOpen = (city?: City) => {
    if (city) {
      setEditingCity(city);
      setFormData({ name: city.name || '', countryId: city.countryId.toString() });
    } else {
      setEditingCity(null);
      setFormData({ name: '', countryId: '' });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingCity(null);
    setFormData({ name: '', countryId: '' });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingCity) {
        await citiesApi.update(editingCity.id, { 
          ...editingCity, 
          name: formData.name,
          countryId: parseInt(formData.countryId)
        });
      } else {
        const selectedCountry = countries.find(c => c.id === parseInt(formData.countryId));
        if (!selectedCountry) {
          throw new Error('Selected country not found');
        }
        await citiesApi.create({ 
          name: formData.name,
          countryId: parseInt(formData.countryId),
          country: selectedCountry,
          hotels: []
        });
      }
      handleClose();
      fetchCities();
    } catch (error) {
      console.error('Error saving city:', error);
      setError('Failed to save city');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this city?')) {
      try {
        await citiesApi.delete(id);
        fetchCities();
      } catch (error) {
        console.error('Error deleting city:', error);
        setError('Failed to delete city');
      }
    }
  };

  const getCountryName = (countryId: number) => {
    const country = countries.find(c => c.id === countryId);
    return country ? country.name : '';
  };

  return (
    <Box>
      <Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
          Add City
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Country</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {cities.map((city) => (
              <TableRow key={city.id}>
                <TableCell>{city.name}</TableCell>
                <TableCell>{getCountryName(city.countryId)}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpen(city)} color="primary">
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(city.id)} color="error">
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>
          {editingCity ? 'Edit City' : 'Add City'}
        </DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              label="Name"
              fullWidth
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              required
            />
            <FormControl fullWidth margin="dense">
              <InputLabel>Country</InputLabel>
              <Select
                value={formData.countryId}
                onChange={(e) => setFormData({ ...formData, countryId: e.target.value })}
                required
              >
                {countries.map((country) => (
                  <MenuItem key={country.id} value={country.id}>
                    {country.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit" variant="contained" color="primary">
              Save
            </Button>
          </DialogActions>
        </form>
      </Dialog>

      <Snackbar
        open={!!error}
        autoHideDuration={6000}
        onClose={() => setError(null)}
      >
        <Alert onClose={() => setError(null)} severity="error">
          {error}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default Cities; 