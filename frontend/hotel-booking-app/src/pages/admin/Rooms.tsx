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
import { roomsApi, hotelsApi } from '../../services/api';
import { Room, HotelDto } from '../../types/api.types';

const Rooms: React.FC = () => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [hotels, setHotels] = useState<HotelDto[]>([]);
  const [open, setOpen] = useState(false);
  const [editingRoom, setEditingRoom] = useState<Room | null>(null);
  const [formData, setFormData] = useState({
    roomNumber: '',
    type: '',
    pricePerNight: '',
    isAvailable: true,
    hotelId: '',
  });
  const [error, setError] = useState<string | null>(null);

  const fetchRooms = async () => {
    try {
      const data = await roomsApi.getAll();
      setRooms(data);
    } catch (error) {
      console.error('Error fetching rooms:', error);
      setError('Failed to fetch rooms');
    }
  };

  const fetchHotels = async () => {
    try {
      const data = await hotelsApi.getAll();
      setHotels(data);
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
        roomNumber: room.roomNumber || '',
        type: room.type || '',
        pricePerNight: room.pricePerNight.toString(),
        isAvailable: room.isAvailable,
        hotelId: room.hotelId.toString(),
      });
    } else {
      setEditingRoom(null);
      setFormData({
        roomNumber: '',
        type: '',
        pricePerNight: '',
        isAvailable: true,
        hotelId: '',
      });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingRoom(null);
    setFormData({
      roomNumber: '',
      type: '',
      pricePerNight: '',
      isAvailable: true,
      hotelId: '',
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const selectedHotel = hotels.find(h => h.id === parseInt(formData.hotelId));
      if (!selectedHotel) {
        throw new Error('Selected hotel not found');
      }

      const roomData = editingRoom
        ? {
            id: editingRoom.id,
            roomNumber: formData.roomNumber,
            type: formData.type,
            pricePerNight: parseFloat(formData.pricePerNight),
            isAvailable: formData.isAvailable,
            hotelId: parseInt(formData.hotelId),
          }
        : {
            roomNumber: formData.roomNumber,
            type: formData.type,
            pricePerNight: parseFloat(formData.pricePerNight),
            isAvailable: formData.isAvailable,
            hotelId: parseInt(formData.hotelId),
          };

      if (editingRoom) {
        await roomsApi.update(editingRoom.id, roomData);
      } else {
        await roomsApi.create(roomData);
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
        await roomsApi.delete(id);
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
                <TableCell>{room.roomNumber}</TableCell>
                <TableCell>{room.type}</TableCell>
                <TableCell>${room.pricePerNight}</TableCell>
                <TableCell>{room.isAvailable ? 'Yes' : 'No'}</TableCell>
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
              value={formData.roomNumber}
              onChange={(e) => setFormData({ ...formData, roomNumber: e.target.value })}
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
                  checked={formData.isAvailable}
                  onChange={(e) => setFormData({ ...formData, isAvailable: e.target.checked })}
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