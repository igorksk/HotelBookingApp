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
  Switch,
  FormControlLabel,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import axios from 'axios';

interface Room {
  id: number;
  number: string;
  type: string;
  pricePerNight: number;
  available: boolean;
  hotelId: number;
}

interface Hotel {
  id: number;
  name: string;
  address: string;
  rating: number;
  cityId: number;
}

const Rooms: React.FC = () => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [hotels, setHotels] = useState<Hotel[]>([]);
  const [open, setOpen] = useState(false);
  const [editingRoom, setEditingRoom] = useState<Room | null>(null);
  const [formData, setFormData] = useState({
    number: '',
    type: '',
    pricePerNight: '',
    available: true,
    hotelId: '',
  });
  const [error, setError] = useState<string | null>(null);

  const fetchRooms = async () => {
    try {
      const response = await axios.get('/api/rooms');
      setRooms(response.data);
    } catch (error) {
      console.error('Error fetching rooms:', error);
      setError('Failed to fetch rooms');
    }
  };

  const fetchHotels = async () => {
    try {
      const response = await axios.get('/api/hotels');
      setHotels(response.data);
    } catch (error) {
      console.error('Error fetching hotels:', error);
      setError('Failed to fetch hotels');
    }
  };

  useEffect(() => {
    fetchRooms();
    fetchHotels();
  }, []);

  const handleOpen = (room?: Room) => {
    if (room) {
      setEditingRoom(room);
      setFormData({
        number: room.number,
        type: room.type,
        pricePerNight: room.pricePerNight.toString(),
        available: room.available,
        hotelId: room.hotelId.toString(),
      });
    } else {
      setEditingRoom(null);
      setFormData({
        number: '',
        type: '',
        pricePerNight: '',
        available: true,
        hotelId: '',
      });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingRoom(null);
    setFormData({
      number: '',
      type: '',
      pricePerNight: '',
      available: true,
      hotelId: '',
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const data = {
        ...formData,
        pricePerNight: parseFloat(formData.pricePerNight),
        hotelId: parseInt(formData.hotelId),
      };

      if (editingRoom) {
        await axios.put(`/api/rooms/${editingRoom.id}`, data);
      } else {
        await axios.post('/api/rooms', data);
      }
      handleClose();
      fetchRooms();
    } catch (error) {
      console.error('Error saving room:', error);
      setError('Failed to save room');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this room?')) {
      try {
        await axios.delete(`/api/rooms/${id}`);
        fetchRooms();
      } catch (error) {
        console.error('Error deleting room:', error);
        setError('Failed to delete room');
      }
    }
  };

  const getHotelName = (hotelId: number) => {
    const hotel = hotels.find(h => h.id === hotelId);
    return hotel ? hotel.name : '';
  };

  return (
    <Box>
      <Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
          Add Room
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Number</TableCell>
              <TableCell>Type</TableCell>
              <TableCell>Price per Night</TableCell>
              <TableCell>Available</TableCell>
              <TableCell>Hotel</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {rooms.map((room) => (
              <TableRow key={room.id}>
                <TableCell>{room.number}</TableCell>
                <TableCell>{room.type}</TableCell>
                <TableCell>${room.pricePerNight}</TableCell>
                <TableCell>{room.available ? 'Yes' : 'No'}</TableCell>
                <TableCell>{getHotelName(room.hotelId)}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpen(room)} color="primary">
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(room.id)} color="error">
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
          {editingRoom ? 'Edit Room' : 'Add Room'}
        </DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              label="Room Number"
              fullWidth
              value={formData.number}
              onChange={(e) => setFormData({ ...formData, number: e.target.value })}
              required
            />
            <TextField
              margin="dense"
              label="Type"
              fullWidth
              value={formData.type}
              onChange={(e) => setFormData({ ...formData, type: e.target.value })}
              required
            />
            <TextField
              margin="dense"
              label="Price per Night"
              type="number"
              fullWidth
              value={formData.pricePerNight}
              onChange={(e) => setFormData({ ...formData, pricePerNight: e.target.value })}
              inputProps={{ min: 0, step: 0.01 }}
              required
            />
            <FormControlLabel
              control={
                <Switch
                  checked={formData.available}
                  onChange={(e) => setFormData({ ...formData, available: e.target.checked })}
                />
              }
              label="Available"
            />
            <FormControl fullWidth margin="dense">
              <InputLabel>Hotel</InputLabel>
              <Select
                value={formData.hotelId}
                onChange={(e) => setFormData({ ...formData, hotelId: e.target.value })}
                required
              >
                {hotels.map((hotel) => (
                  <MenuItem key={hotel.id} value={hotel.id}>
                    {hotel.name}
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

export default Rooms; 