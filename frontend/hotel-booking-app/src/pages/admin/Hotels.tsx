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
import { HotelDto, City } from '../../types/api.types';
import { hotelsApi, citiesApi } from '../../services/api';

const Hotels: React.FC = () => {
  const [hotels, setHotels] = useState<HotelDto[]>([]);
  const [cities, setCities] = useState<City[]>([]);
  const [open, setOpen] = useState(false);
  const [editingHotel, setEditingHotel] = useState<HotelDto | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    address: '',
    rating: '',
    cityId: '',
  });
  const [error, setError] = useState<string | null>(null);

  const fetchHotels = async () => {
    try {
      const data = await hotelsApi.getAll();
      setHotels(data);
    } catch (error) {
      console.error('Error fetching hotels:', error);
      setError('Failed to fetch hotels');
    }
  };

  const fetchCities = async () => {
    try {
      const data = await citiesApi.getAll();
      setCities(data);
    } catch (error) {
      console.error('Error fetching cities:', error);
      setError('Failed to fetch cities');
    }
  };

  useEffect(() => {
    fetchHotels();
    fetchCities();
  }, []);

  const handleOpen = (hotel?: HotelDto) => {
    if (hotel) {
      setEditingHotel(hotel);
      setFormData({
        name: hotel.name || '',
        address: hotel.address || '',
        rating: hotel.rating.toString(),
        cityId: hotel.cityId.toString(),
      });
    } else {
      setEditingHotel(null);
      setFormData({
        name: '',
        address: '',
        rating: '',
        cityId: '',
      });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingHotel(null);
    setFormData({
      name: '',
      address: '',
      rating: '',
      cityId: '',
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const data = editingHotel
        ? {
            id: editingHotel.id,
            name: formData.name,
            address: formData.address,
            rating: parseFloat(formData.rating),
            cityId: parseInt(formData.cityId),
          }
        : {
            name: formData.name,
            address: formData.address,
            rating: parseFloat(formData.rating),
            cityId: parseInt(formData.cityId),
          };

      if (editingHotel) {
        await hotelsApi.update(editingHotel.id, data);
      } else {
        await hotelsApi.create(data);
      }
      handleClose();
      fetchHotels();
    } catch (error) {
      console.error('Error saving hotel:', error);
      setError('Failed to save hotel');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this hotel?')) {
      try {
        await hotelsApi.delete(id);
        fetchHotels();
      } catch (error) {
        console.error('Error deleting hotel:', error);
        setError('Failed to delete hotel');
      }
    }
  };

  const getCityName = (cityId: number) => {
    const city = cities.find(c => c.id === cityId);
    return city?.name || '';
  };

  return (
    <Box>
      <Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
          Add Hotel
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Address</TableCell>
              <TableCell>Rating</TableCell>
              <TableCell>City</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {hotels.map((hotel) => (
              <TableRow key={hotel.id}>
                <TableCell>{hotel.name}</TableCell>
                <TableCell>{hotel.address}</TableCell>
                <TableCell>{hotel.rating}</TableCell>
                <TableCell>{hotel.cityName}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpen(hotel)} color="primary">
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(hotel.id)} color="error">
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
          {editingHotel ? 'Edit Hotel' : 'Add Hotel'}
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
            <TextField
              margin="dense"
              label="Address"
              fullWidth
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              required
            />
            <TextField
              margin="dense"
              label="Rating"
              type="number"
              fullWidth
              value={formData.rating}
              onChange={(e) => setFormData({ ...formData, rating: e.target.value })}
              inputProps={{ min: 0, max: 5, step: 0.1 }}
              required
            />
            <FormControl fullWidth margin="dense">
              <InputLabel>City</InputLabel>
              <Select
                value={formData.cityId}
                onChange={(e) => setFormData({ ...formData, cityId: e.target.value })}
                required
              >
                {cities.map((city) => (
                  <MenuItem key={city.id} value={city.id}>
                    {city.name}
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

export default Hotels; 