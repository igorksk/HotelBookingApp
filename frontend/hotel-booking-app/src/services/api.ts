import axios from 'axios';
import { HotelDto, Room, RoomDto, Booking, BookingDto, Country, City } from '../types/api.types';

const API_BASE_URL = 'http://localhost:7263';

// Тип для передачи данных о комнате на сервер
export type RoomPayload = {
  id?: number;
  roomNumber: string;
  type: string;
  pricePerNight: number;
  isAvailable: boolean;
  hotelId: number;
};

export const hotelsApi = {
  getAll: async (params?: { country?: string; city?: string; checkIn?: string; checkOut?: string }) => {
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

export const roomsApi = {
  getAll: async () => {
    const response = await axios.get<Room[]>(`${API_BASE_URL}/api/Rooms`);
    return response.data;
  },

  getById: async (id: number) => {
    const response = await axios.get<Room>(`${API_BASE_URL}/api/Rooms/${id}`);
    return response.data;
  },

  create: async (room: RoomPayload) => {
    const response = await axios.post<Room>(`${API_BASE_URL}/api/Rooms`, room);
    return response.data;
  },

  update: async (id: number, room: RoomPayload) => {
    const response = await axios.put(`${API_BASE_URL}/api/Rooms/${id}`, room);
    return response.data;
  },

  delete: async (id: number) => {
    await axios.delete(`${API_BASE_URL}/api/Rooms/${id}`);
  }
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

export const countriesApi = {
  getAll: async () => {
    const response = await axios.get<Country[]>(`${API_BASE_URL}/api/Countries`);
    return response.data;
  },

  getById: async (id: number) => {
    const response = await axios.get<Country>(`${API_BASE_URL}/api/Countries/${id}`);
    return response.data;
  },

  create: async (country: Omit<Country, 'id'>) => {
    const response = await axios.post<Country>(`${API_BASE_URL}/api/Countries`, country);
    return response.data;
  },

  update: async (id: number, country: Country) => {
    const response = await axios.put(`${API_BASE_URL}/api/Countries/${id}`, country);
    return response.data;
  },

  delete: async (id: number) => {
    await axios.delete(`${API_BASE_URL}/api/Countries/${id}`);
  }
};

export const citiesApi = {
  getAll: async (countryId?: number) => {
    const response = await axios.get<City[]>(`${API_BASE_URL}/api/Cities`, {
      params: { countryId }
    });
    return response.data;
  },

  getById: async (id: number) => {
    const response = await axios.get<City>(`${API_BASE_URL}/api/Cities/${id}`);
    return response.data;
  },

  create: async (city: Omit<City, 'id'>) => {
    const response = await axios.post<City>(`${API_BASE_URL}/api/Cities`, city);
    return response.data;
  },

  update: async (id: number, city: City) => {
    const response = await axios.put(`${API_BASE_URL}/api/Cities/${id}`, city);
    return response.data;
  },

  delete: async (id: number) => {
    await axios.delete(`${API_BASE_URL}/api/Cities/${id}`);
  }
};

export default {
  hotels: hotelsApi,
  bookings: bookingsApi,
  countries: countriesApi,
  cities: citiesApi,
  rooms: roomsApi,
}; 