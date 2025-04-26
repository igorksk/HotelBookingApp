import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { Box, Drawer, List, ListItem, ListItemIcon, ListItemText, AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Hotel, LocationCity, Public, MeetingRoom, ExitToApp } from '@mui/icons-material';

const drawerWidth = 240;

const AdminLayout: React.FC = () => {
  const navigate = useNavigate();

  const handleExit = () => {
    navigate('/');
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
        <Toolbar>
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
            Admin Panel
          </Typography>
          <Button
            color="inherit"
            onClick={handleExit}
            startIcon={<ExitToApp />}
          >
            Exit
          </Button>
        </Toolbar>
      </AppBar>
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
      >
        <Toolbar />
        <List>
          <ListItem component={Link} to="/admin/countries">
            <ListItemIcon>
              <Public />
            </ListItemIcon>
            <ListItemText primary="Countries" />
          </ListItem>
          <ListItem component={Link} to="/admin/cities">
            <ListItemIcon>
              <LocationCity />
            </ListItemIcon>
            <ListItemText primary="Cities" />
          </ListItem>
          <ListItem component={Link} to="/admin/hotels">
            <ListItemIcon>
              <Hotel />
            </ListItemIcon>
            <ListItemText primary="Hotels" />
          </ListItem>
          <ListItem component={Link} to="/admin/rooms">
            <ListItemIcon>
              <MeetingRoom />
            </ListItemIcon>
            <ListItemText primary="Rooms" />
          </ListItem>
        </List>
      </Drawer>
      <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
        <Toolbar />
        <Outlet />
      </Box>
    </Box>
  );
};

export default AdminLayout; 