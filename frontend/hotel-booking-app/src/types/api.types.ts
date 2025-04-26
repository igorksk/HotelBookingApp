export interface Hotel {
  id: number;
  name: string | null;
  address: string | null;
  cityId: number;
  city: City;
  rating: number;
  rooms: Room[] | null;
}

export interface HotelDto {
  id: number;
  name: string | null;
  address: string | null;
  rating: number;
  cityId: number;
  cityName: string | null;
  countryName: string | null;
  countryCode: string | null;
  rooms: RoomDto[] | null;
}

export interface Room {
  id: number;
  roomNumber: string | null;
  type: string | null;
  pricePerNight: number;
  isAvailable: boolean;
  hotelId: number;
  hotel: Hotel;
  bookings: Booking[] | null;
}

export interface RoomDto {
  id: number;
  roomNumber: string | null;
  type: string | null;
  pricePerNight: number;
  isAvailable: boolean;
}

export interface Booking {
  id: number;
  guestName: string | null;
  guestEmail: string | null;
  checkInDate: string;
  checkOutDate: string;
  roomId: number;
  room: Room;
  totalPrice: number;
  status: string | null;
}

export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
}

export interface BookingDto {
  id: number;
  guestName: string | null;
  guestEmail: string | null;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: string | null;
  roomNumber: string | null;
  roomType: string | null;
  pricePerNight: number;
  hotelName: string | null;
  hotelAddress: string | null;
  hotelCity: string | null;
  hotelCountry: string | null;
  countryCode: string | null;
}

export interface City {
  id: number;
  name: string | null;
  countryId: number;
  country: Country;
  hotels: Hotel[] | null;
}

export interface Country {
  id: number;
  name: string | null;
  code: string | null;
  cities: City[] | null;
} 