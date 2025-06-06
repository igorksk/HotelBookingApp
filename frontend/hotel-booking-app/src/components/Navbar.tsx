import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { AdminPanelSettings } from '@mui/icons-material';

const Navbar = () => {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          Hotel Booking
        </Typography>
        <Box>
          <Button color="inherit" component={RouterLink} to="/">
            Home
          </Button>
          <Button color="inherit" component={RouterLink} to="/hotels">
            Hotels
          </Button>
          <Button color="inherit" component={RouterLink} to="/my-bookings">
            My Bookings
          </Button>
          <Button
            color="inherit"
            component={RouterLink}
            to="/admin"
            startIcon={<AdminPanelSettings />}
          >
            Admin
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar; 