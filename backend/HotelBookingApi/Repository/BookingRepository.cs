using HotelBookingApi.Data;
using HotelBookingApi.DTOs;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class BookingRepository(HotelBookingContext context) : IBookingRepository
    {
        private readonly HotelBookingContext _context = context;

        #region Read Methods

        public IQueryable<BookingDto> GetBookings()
        {
            return _context.Bookings
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
        }

        public async Task<BookingDto?> GetBooking(int id)
        {
            return await GetBookings()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Booking?> GetBookingWithRoomAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> BookingExistsAsync(int id)
        {
            return await _context.Bookings.AnyAsync(b => b.Id == id);
        }

        public async Task<Room?> GetRoomByIdAsync(int roomId)
        {
            return await _context.Rooms.FindAsync(roomId);
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

        #endregion

        #region Write Methods

        public async Task CreateBookingAsync(Booking booking, Room room)
        {
            _context.Bookings.Add(booking);
            room.IsAvailable = false;
            await SaveChangesAsync();
        }

        public void UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
        }

        public void RemoveBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
