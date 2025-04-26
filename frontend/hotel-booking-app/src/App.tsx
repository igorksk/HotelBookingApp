import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider, CssBaseline } from '@mui/material';
import theme from './theme';
import Home from './pages/Home';
import HotelList from './pages/HotelList';
import Booking from './pages/Booking';
import MyBookings from './pages/MyBookings';
import Navbar from './components/Navbar';
import AdminLayout from './pages/admin/AdminLayout';
import Countries from './pages/admin/Countries';
import Cities from './pages/admin/Cities';
import Hotels from './pages/admin/Hotels';
import Rooms from './pages/admin/Rooms';

const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/hotels" element={<HotelList />} />
          <Route path="/booking/:hotelId/:roomId" element={<Booking />} />
          <Route path="/my-bookings" element={<MyBookings />} />
          
          {/* Admin routes */}
          <Route path="/admin" element={<AdminLayout />}>
            <Route path="countries" element={<Countries />} />
            <Route path="cities" element={<Cities />} />
            <Route path="hotels" element={<Hotels />} />
            <Route path="rooms" element={<Rooms />} />
          </Route>
        </Routes>
      </Router>
    </ThemeProvider>
  );
};

export default App;
