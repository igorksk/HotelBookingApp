using HotelBookingApi.Data;
using HotelBookingApi.DTOs;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class HotelRepository(HotelBookingContext context) : IHotelRepository
    {
        public async Task<IEnumerable<HotelDto>> GetHotelsAsync(string? country, string? city, DateTime? checkIn, DateTime? checkOut)
        {
            var query = context.Hotels
                .Include(h => h.City).ThenInclude(c => c.Country)
                .Include(h => h.Rooms).ThenInclude(r => r.Bookings)
                .AsQueryable();

            if (!string.IsNullOrEmpty(country))
                query = query.Where(h => h.City.Country.Name == country);
            if (!string.IsNullOrEmpty(city))
                query = query.Where(h => h.City.Name == city);

            var hotels = await query.ToListAsync();

            if (checkIn.HasValue && checkOut.HasValue)
            {
                hotels = [.. hotels.Where(h => h.Rooms.Any(r =>
                    r.IsAvailable &&
                    !r.Bookings.Any(b => checkIn < b.CheckOutDate && checkOut > b.CheckInDate)
                ))];
            }

            return [.. hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Rating = h.Rating,
                CityId = h.CityId,
                CityName = h.City?.Name ?? "",
                CountryName = h.City?.Country?.Name ?? "",
                CountryCode = h.City?.Country?.Code ?? "",
                Rooms = [.. h.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable
                })]
            })];
        }

        public async Task<HotelDto?> GetHotelAsync(int id)
        {
            var hotel = await context.Hotels
                .Include(h => h.City).ThenInclude(c => c.Country)
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null) return null;

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name ?? "",
                Address = hotel.Address ?? "",
                Rating = hotel.Rating,
                CityId = hotel.CityId,
                CityName = hotel.City?.Name ?? "",
                CountryName = hotel.City?.Country?.Name ?? "",
                CountryCode = hotel.City?.Country?.Code ?? "",
                Rooms = [.. hotel.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable
                })]
            };
        }

        public async Task<Hotel?> CreateHotelAsync(CreateHotelDto dto)
        {
            var city = await context.Cities.FindAsync(dto.CityId);
            if (city == null) return null;

            var hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                Rating = dto.Rating,
                CityId = dto.CityId,
                City = city
            };

            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();
            return hotel;
        }

        public async Task<bool> UpdateHotelAsync(int id, UpdateHotelDto dto)
        {
            if (id != dto.Id) return false;

            var hotel = await context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            var city = await context.Cities.FindAsync(dto.CityId);
            if (city == null) return false;

            hotel.Name = dto.Name;
            hotel.Address = dto.Address;
            hotel.Rating = dto.Rating;
            hotel.CityId = dto.CityId;
            hotel.City = city;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            context.Hotels.Remove(hotel);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await context.Cities.AnyAsync(c => c.Id == cityId);
        }
    }
}
