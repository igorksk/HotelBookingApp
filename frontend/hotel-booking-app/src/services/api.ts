import axios from 'axios';
import { HotelDto, Room, RoomDto, Booking, BookingDto, Country, City } from '../types/api.types';

const api = axios.create({
  baseURL: 'http://localhost:7263/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Type for sending room data to the server
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
    const response = await api.get<HotelDto[]>('/Hotels', { params });
    return response.data;
  },

  getById: async (id: number) => {
    const response = await api.get<HotelDto>(`/Hotels/${id}`);
    return response.data;
  },

  getRooms: async (hotelId: number) => {
    const response = await api.get<RoomDto[]>('/Rooms', {
      params: { hotelId }
    });
    return response.data;
  },

  create: async (hotel: any) => {
    const response = await api.post('/Hotels', hotel);
    return response.data;
  },

  update: async (id: number, hotel: any) => {
    const response = await api.put(`/Hotels/${id}`, hotel);
    return response.data;
  },

  delete: async (id: number) => {
    await api.delete(`/Hotels/${id}`);
  }
};

export const roomsApi = {
  getAll: async () => {
    const response = await api.get<Room[]>('/Rooms');
    return response.data;
  },

  getById: async (id: number) => {
    const response = await api.get<Room>(`/Rooms/${id}`);
    return response.data;
  },

  create: async (room: RoomPayload) => {
    const response = await api.post<Room>('/Rooms', room);
    return response.data;
  },

  update: async (id: number, room: RoomPayload) => {
    const response = await api.put(`/Rooms/${id}`, room);
    return response.data;
  },

  delete: async (id: number) => {
    await api.delete(`/Rooms/${id}`);
  }
};

export const bookingsApi = {
  create: async (booking: Omit<Booking, 'id' | 'room' | 'totalPrice'>) => {
    const response = await api.post<BookingDto>('/Bookings', booking);
    return response.data;
  },

  getByUser: async (userId: number) => {
    const response = await api.get<BookingDto[]>('/Bookings', {
      params: { userId }
    });
    return response.data;
  },

  cancel: async (bookingId: number) => {
    await api.delete(`/Bookings/${bookingId}`);
  },
};

export const countriesApi = {
  getAll: async () => {
    const response = await api.get<Country[]>('/Countries');
    return response.data;
  },

  getById: async (id: number) => {
    const response = await api.get<Country>(`/Countries/${id}`);
    return response.data;
  },

  create: async (country: Omit<Country, 'id'>) => {
    const response = await api.post<Country>('/Countries', country);
    return response.data;
  },

  update: async (id: number, country: Country) => {
    const response = await api.put(`/Countries/${id}`, country);
    return response.data;
  },

  delete: async (id: number) => {
    await api.delete(`/Countries/${id}`);
  }
};

export const citiesApi = {
  getAll: async (countryId?: number) => {
    const response = await api.get<City[]>('/Cities', {
      params: { countryId }
    });
    return response.data;
  },

  getById: async (id: number) => {
    const response = await api.get<City>(`/Cities/${id}`);
    return response.data;
  },

  create: async (city: Omit<City, 'id'>) => {
    const response = await api.post<City>('/Cities', city);
    return response.data;
  },

  update: async (id: number, city: City) => {
    const response = await api.put(`/Cities/${id}`, city);
    return response.data;
  },

  delete: async (id: number) => {
    await api.delete(`/Cities/${id}`);
  }
};

export default {
  hotels: hotelsApi,
  bookings: bookingsApi,
  countries: countriesApi,
  cities: citiesApi,
  rooms: roomsApi,
}; 