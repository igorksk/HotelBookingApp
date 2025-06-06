﻿using HotelBookingApi.Data;
using HotelBookingApi.DTOs;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class BookingRepository(HotelBookingContext context) : IBookingRepository
    {
        private readonly HotelBookingContext _context = context;

        public IQueryable<BookingDto> GetBookings()
        {
            var bookings = _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                        .ThenInclude(h => h.City)
                            .ThenInclude(c => c.Country)
                .OrderByDescending(b => b.CheckInDate)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    GuestName = b.GuestName,
                    GuestEmail = b.GuestEmail,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    RoomNumber = b.Room.RoomNumber,
                    RoomType = b.Room.Type,
                    PricePerNight = b.Room.PricePerNight,
                    HotelName = b.Room.Hotel.Name,
                    HotelAddress = b.Room.Hotel.Address,
                    HotelCity = b.Room.Hotel.City.Name,
                    HotelCountry = b.Room.Hotel.City.Country.Name,
                    CountryCode = b.Room.Hotel.City.Country.Code
                });

            return bookings;
        }

        public async Task<BookingDto?> GetBooking(int id)
        {
            var booking = await GetBookings()
                .FirstOrDefaultAsync(b => b.Id == id);

            return booking;
        }

        public async Task<bool> IsRoomBookedAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            return await _context.Bookings
                .Where(b => b.RoomId == roomId && (excludeBookingId == null || b.Id != excludeBookingId))
                .AnyAsync(b =>
                    (checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||
                    (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
                    (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate));
        }

        // TO DO: add other methods
    }
}
