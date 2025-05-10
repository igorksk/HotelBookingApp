using HotelBookingApi.Data;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController(HotelBookingContext context, ILogger<CitiesController> logger) : ControllerBase
{
    private readonly HotelBookingContext _context = context;
    private readonly ILogger<CitiesController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities([FromQuery] int? countryId)
    {
        try
        {
            _logger.LogInformation("Getting cities for country ID: {CountryId}", countryId);
            var citiesQuery = _context.Cities
                .Include(c => c.Country)
                .AsQueryable();

            if (countryId.HasValue)
            {
                citiesQuery = citiesQuery.Where(c => c.CountryId == countryId);
            }

            var cities = await citiesQuery
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting cities");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(int id)
    {
        var city = await _context.Cities
            .Include(c => c.Country)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        return city;
    }

    [HttpPost]
    public async Task<ActionResult<City>> PostCity(City city)
    {
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(int id, City city)
    {
        if (id != city.Id)
        {
            return BadRequest();
        }

        _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null)
        {
            return NotFound();
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(int id)
    {
        return _context.Cities.Any(e => e.Id == id);
    }
} 