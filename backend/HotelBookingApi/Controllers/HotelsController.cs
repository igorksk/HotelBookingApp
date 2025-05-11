using HotelBookingApi.Data;
using HotelBookingApi.DTOs;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(HotelBookingContext context, ILogger<HotelsController> logger) : ControllerBase
{
    private readonly HotelBookingContext _context = context;
    private readonly ILogger<HotelsController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels(
        [FromQuery] string? country,
        [FromQuery] string? city,
        [FromQuery] DateTime? checkIn,
        [FromQuery] DateTime? checkOut)
    {
        try
        {
            _logger.LogInformation("Getting hotel list (filters: country={Country}, city={City})", country, city);

            var hotelsQuery = _context.Hotels
                .Include(h => h.City)
                    .ThenInclude(c => c.Country)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.Bookings)
                .AsQueryable();

            if (!string.IsNullOrEmpty(country))
                hotelsQuery = hotelsQuery.Where(h => h.City.Country.Name == country);

            if (!string.IsNullOrEmpty(city))
                hotelsQuery = hotelsQuery.Where(h => h.City.Name == city);

            var hotels = await hotelsQuery.ToListAsync();

            // Filter by dates: keep only hotels with at least one available room
            if (checkIn.HasValue && checkOut.HasValue)
            {
                hotels = [.. hotels
                    .Where(h => h.Rooms.Any(r =>
                        r.IsAvailable &&
                        !r.Bookings.Any(b =>
                            (checkIn < b.CheckOutDate && checkOut > b.CheckInDate)
                        )
                    ))];
            }

            var hotelDtos = hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Rating = h.Rating,
                CityId = h.CityId,
                CityName = h.City?.Name ?? string.Empty,
                CountryName = h.City?.Country?.Name ?? string.Empty,
                CountryCode = h.City?.Country?.Code ?? string.Empty,
                Rooms = [.. h.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable
                })]
            }).ToList();

            return Ok(hotelDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting hotel list");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        try
        {
            _logger.LogInformation("Getting hotel by id={HotelId}", id);

            var hotel = await _context.Hotels
                .Include(h => h.City)
                    .ThenInclude(c => c.Country)
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDto = new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name ?? string.Empty,
                Address = hotel.Address ?? string.Empty,
                Rating = hotel.Rating,
                CityName = hotel.City?.Name ?? string.Empty,
                CountryName = hotel.City?.Country?.Name ?? string.Empty,
                CountryCode = hotel.City?.Country?.Code ?? string.Empty,
                CityId = hotel.CityId,
                Rooms = hotel.Rooms?.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable
                }).ToList() ?? new List<RoomDto>()
            };

            return Ok(hotelDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting hotel by id={HotelId}", id);
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotelDto)
    {
        _logger.LogInformation("Creating new hotel: {Name}", createHotelDto?.Name);

        if (createHotelDto == null)
        {
            return BadRequest("Hotel data is required");
        }

        if (string.IsNullOrWhiteSpace(createHotelDto.Name))
        {
            return BadRequest("Hotel name is required");
        }

        if (createHotelDto.CityId <= 0)
        {
            return BadRequest("Valid CityId is required");
        }

        // Проверяем существование города
        var city = await _context.Cities.FindAsync(createHotelDto.CityId);
        if (city == null)
        {
            return BadRequest("Specified city does not exist");
        }

        var hotel = new Hotel
        {
            Name = createHotelDto.Name,
            Address = createHotelDto.Address,
            Rating = createHotelDto.Rating,
            CityId = createHotelDto.CityId,
            City = city
        };

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updateHotelDto)
    {
        _logger.LogInformation("Updating hotel id={HotelId}", id);

        if (id != updateHotelDto.Id)
        {
            return BadRequest("Id mismatch");
        }

        if (updateHotelDto == null)
        {
            return BadRequest("Hotel data is required");
        }

        if (string.IsNullOrWhiteSpace(updateHotelDto.Name))
        {
            return BadRequest("Hotel name is required");
        }

        if (updateHotelDto.CityId <= 0)
        {
            return BadRequest("Valid CityId is required");
        }

        // Проверяем существование города
        var city = await _context.Cities.FindAsync(updateHotelDto.CityId);
        if (city == null)
        {
            return BadRequest("Specified city does not exist");
        }

        var existingHotel = await _context.Hotels.FindAsync(id);
        if (existingHotel == null)
        {
            return NotFound();
        }

        existingHotel.Name = updateHotelDto.Name;
        existingHotel.Address = updateHotelDto.Address;
        existingHotel.Rating = updateHotelDto.Rating;
        existingHotel.CityId = updateHotelDto.CityId;
        existingHotel.City = city;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HotelExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        _logger.LogInformation("Deleting hotel id={HotelId}", id);

        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }

        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool HotelExists(int id)
    {
        return _context.Hotels.Any(e => e.Id == id);
    }
}