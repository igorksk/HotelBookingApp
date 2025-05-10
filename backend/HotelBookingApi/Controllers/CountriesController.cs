using HotelBookingApi.Data;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(HotelBookingContext context, ILogger<CountriesController> logger) : ControllerBase
{
    private readonly HotelBookingContext _context = context;
    private readonly ILogger<CountriesController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        try
        {
            _logger.LogInformation("Getting all available countries");
            var countries = await _context.Countries
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting countries");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }

        return country;
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(Country country)
    {
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, Country country)
    {
        if (id != country.Id)
        {
            return BadRequest();
        }

        _context.Entry(country).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound();
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CountryExists(int id)
    {
        return _context.Countries.Any(e => e.Id == id);
    }
} 