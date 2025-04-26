using HotelBookingApi.Models;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface ICityRepository
    {
        Task<List<City>> GetCitiesAsync(int? countryId);
        Task<City?> GetCityAsync(int id);
        Task<City> AddCityAsync(City city);
        Task<bool> UpdateCityAsync(City city);
        Task<bool> DeleteCityAsync(int id);
        Task<bool> CityExistsAsync(int id);
    }
}
