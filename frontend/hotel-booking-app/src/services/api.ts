import axios from 'axios';
import { Hotel, HotelDto, Room, RoomDto, Booking, BookingDto } from '../types/api.types';

const API_BASE_URL = 'https://localhost:7263';

export const hotelsApi = {
  getAll: async (params?: { location?: string; checkIn?: string; checkOut?: string }) => {
    const response = await axios.get<HotelDto[]>(`${API_BASE_URL}/api/Hotels`, { params });
    return response.data;
  },

  getById: async (id: number) => {
    const response = await axios.get<HotelDto>(`${API_BASE_URL}/api/Hotels/${id}`);
    return response.data;
  },

  getRooms: async (hotelId: number) => {
    const response = await axios.get<RoomDto[]>(`${API_BASE_URL}/api/Rooms`, {
      params: { hotelId }
    });
    return response.data;
  },
};

export const bookingsApi = {
  create: async (booking: Omit<Booking, 'id' | 'room' | 'totalPrice'>) => {
    const response = await axios.post<BookingDto>(`${API_BASE_URL}/api/Bookings`, booking);
    return response.data;
  },

  getByUser: async (userId: number) => {
    const response = await axios.get<BookingDto[]>(`${API_BASE_URL}/api/Bookings`, {
      params: { userId }
    });
    return response.data;
  },

  cancel: async (bookingId: number) => {
    const response = await axios.post<BookingDto>(`${API_BASE_URL}/api/Bookings/${bookingId}/cancel`);
    return response.data;
  },
};

export default {
  hotels: hotelsApi,
  bookings: bookingsApi,
}; 