using HotelBookingApi.Data;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class CountryRepository(HotelBookingContext context) : ICountryRepository
    {
        private readonly HotelBookingContext _context = context;

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _context.Countries
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Country?> GetCountryAsync(int id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task<Country> AddCountryAsync(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            _context.Entry(country).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return await CountryExistsAsync(country.Id);
            }
        }

        public async Task<bool> DeleteCountryAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return false;

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> CountryExistsAsync(int id)
        {
            return _context.Countries.AnyAsync(c => c.Id == id);
        }
    }
}
