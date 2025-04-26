using HotelBookingApi.Models;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountriesAsync();
        Task<Country?> GetCountryAsync(int id);
        Task<Country> AddCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> CountryExistsAsync(int id);
    }
}
