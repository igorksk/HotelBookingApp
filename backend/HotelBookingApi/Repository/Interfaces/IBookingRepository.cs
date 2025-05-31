using HotelBookingApi.DTOs;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingDto>> GetBookings();
    }
}
