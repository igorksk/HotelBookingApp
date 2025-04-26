using HotelBookingApi.DTOs;
using HotelBookingApi.Models;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface IBookingRepository
    {
        #region Read Methods

        IQueryable<BookingDto> GetBookings();

        Task<BookingDto?> GetBooking(int id);

        Task<Booking?> GetBookingByIdAsync(int id);

        Task<Booking?> GetBookingWithRoomAsync(int id);

        Task<bool> BookingExistsAsync(int id);

        Task<Room?> GetRoomByIdAsync(int roomId);

        Task<bool> IsRoomBookedAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);

        #endregion

        #region Write Methods

        Task CreateBookingAsync(Booking booking, Room room);

        void UpdateBooking(Booking booking);

        void RemoveBooking(Booking booking);

        Task SaveChangesAsync();

        #endregion
    }
}
