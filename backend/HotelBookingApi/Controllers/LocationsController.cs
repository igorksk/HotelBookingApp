using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Data;
using HotelBookingApi.Models;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController(HotelBookingContext context, ILogger<LocationsController> logger) : ControllerBase
{
    private readonly HotelBookingContext _context = context;
    private readonly ILogger<LocationsController> _logger = logger;

    [HttpGet("countries")]
    public async Task<ActionResult<IEnumerable<string>>> GetCountries()
    {
        try
        {
            _logger.LogInformation("Getting all available countries");
            var countries = await _context.Countries
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting countries");
            throw;
        }
    }

    [HttpGet("cities")]
    public async Task<ActionResult<IEnumerable<string>>> GetCities([FromQuery] string? country)
    {
        try
        {
            _logger.LogInformation("Getting cities for country: {Country}", country);
            var citiesQuery = _context.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(country))
            {
                citiesQuery = citiesQuery.Where(c => c.Country.Name == country);
            }

            var cities = await citiesQuery
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting cities");
            throw;
        }
    }
} 