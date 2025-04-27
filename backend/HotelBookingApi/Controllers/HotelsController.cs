using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Data;
using HotelBookingApi.Models;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly HotelBookingContext _context;
    private readonly ILogger<HotelsController> _logger;

    public HotelsController(HotelBookingContext context, ILogger<HotelsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels(
        [FromQuery] string? country,
        [FromQuery] string? city,
        [FromQuery] DateTime? checkIn,
        [FromQuery] DateTime? checkOut)
    {
        try
        {
            _logger.LogInformation("Getting filtered hotels");

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
                hotels = hotels
                    .Where(h => h.Rooms.Any(r =>
                        r.IsAvailable &&
                        !r.Bookings.Any(b =>
                            (checkIn < b.CheckOutDate && checkOut > b.CheckInDate)
                        )
                    ))
                    .ToList();
            }

            var hotelDtos = hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Rating = h.Rating,
                CityName = h.City.Name,
                CountryName = h.City.Country.Name,
                CountryCode = h.City.Country.Code,
                Rooms = h.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable
                }).ToList()
            }).ToList();

            return Ok(hotelDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting hotels");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _context.Hotels
            .Include(h => h.City)
                .ThenInclude(c => c.Country)
            .Include(h => h.Rooms)
            .Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Rating = h.Rating,
                CityName = h.City.Name,
                CountryName = h.City.Country.Name,
                CountryCode = h.City.Country.Code,
                Rooms = h.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    IsAvailable = r.IsAvailable
                }).ToList()
            })
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            return NotFound();
        }

        return hotel;
    }

    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
    {
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, Hotel hotel)
    {
        if (id != hotel.Id)
        {
            return BadRequest();
        }

        _context.Entry(hotel).State = EntityState.Modified;

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