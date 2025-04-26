using HotelBookingApi.Data;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class CityRepository(HotelBookingContext context) : ICityRepository
    {
        private readonly HotelBookingContext _context = context;

        public async Task<List<City>> GetCitiesAsync(int? countryId)
        {
            var query = _context.Cities.Include(c => c.Country).AsQueryable();

            if (countryId.HasValue)
            {
                query = query.Where(c => c.CountryId == countryId);
            }

            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int id)
        {
            return await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<City> AddCityAsync(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
            return city;
        }

        public async Task<bool> UpdateCityAsync(City city)
        {
            _context.Entry(city).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return await CityExistsAsync(city.Id);
            }
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null) return false;

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> CityExistsAsync(int id)
        {
            return _context.Cities.AnyAsync(c => c.Id == id);
        }
    }
}
