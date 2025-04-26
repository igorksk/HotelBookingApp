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
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { countriesApi } from '../../services/api';
import { Country } from '../../types/api.types';

const Countries: React.FC = () => {
  const [countries, setCountries] = useState<Country[]>([]);
  const [open, setOpen] = useState(false);
  const [editingCountry, setEditingCountry] = useState<Country | null>(null);
  const [formData, setFormData] = useState({ name: '', code: '' });
  const [error, setError] = useState<string | null>(null);

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
    fetchCountries();
  }, []);

  const handleOpen = (country?: Country) => {
    if (country) {
      setEditingCountry(country);
      setFormData({ name: country.name || '', code: country.code || '' });
    } else {
      setEditingCountry(null);
      setFormData({ name: '', code: '' });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingCountry(null);
    setFormData({ name: '', code: '' });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingCountry) {
        await countriesApi.update(editingCountry.id, { ...editingCountry, ...formData });
      } else {
        await countriesApi.create({ ...formData, cities: [] });
      }
      handleClose();
      fetchCountries();
    } catch (error) {
      console.error('Error saving country:', error);
      setError('Failed to save country');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this country?')) {
      try {
        await countriesApi.delete(id);
        fetchCountries();
      } catch (error) {
        console.error('Error deleting country:', error);
        setError('Failed to delete country');
      }
    }
  };

  return (
    <Box>
      <Box sx={{ mb: 2, display: 'flex', justifyContent: 'flex-end' }}>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
          Add Country
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Code</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {countries.map((country) => (
              <TableRow key={country.id}>
                <TableCell>{country.name}</TableCell>
                <TableCell>{country.code}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpen(country)} color="primary">
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(country.id)} color="error">
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
          {editingCountry ? 'Edit Country' : 'Add Country'}
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
              label="Code"
              fullWidth
              value={formData.code}
              onChange={(e) => setFormData({ ...formData, code: e.target.value })}
              required
            />
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

export default Countries; 