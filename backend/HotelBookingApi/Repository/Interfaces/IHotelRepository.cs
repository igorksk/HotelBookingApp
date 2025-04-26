using HotelBookingApi.DTOs;
using HotelBookingApi.Models;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<HotelDto>> GetHotelsAsync(string? country, string? city, DateTime? checkIn, DateTime? checkOut);
        Task<HotelDto?> GetHotelAsync(int id);
        Task<Hotel?> CreateHotelAsync(CreateHotelDto dto);
        Task<bool> UpdateHotelAsync(int id, UpdateHotelDto dto);
        Task<bool> DeleteHotelAsync(int id);
        Task<bool> CityExistsAsync(int cityId);
    }
}
