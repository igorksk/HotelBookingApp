using HotelBookingApi.DTOs;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface IBookingRepository
    {
        IQueryable<BookingDto> GetBookings();

        Task<BookingDto?> GetBooking(int id);

        Task<bool> IsRoomBookedAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
    }
}
